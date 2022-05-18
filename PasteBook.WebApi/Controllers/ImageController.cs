using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PasteBook.Data;
using PasteBook.Data.DataTransferObjects;
using PasteBook.Data.Exceptions;
using PasteBook.Data.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PasteBook.WebApi.Controllers
{
    [Route("Images")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private IWebHostEnvironment Environment;
        private readonly IUnitOfWork UnitOfWork;
        public ImageController(IWebHostEnvironment Environment, IUnitOfWork UnitOfWork)
        {
            this.Environment = Environment;
            this.UnitOfWork = UnitOfWork;
        }

        [HttpGet("GetImage")]
        public async Task<IActionResult> GetImage(int imageId)
        {
            try
            {
                var isExistingImage = await this.UnitOfWork.ImageRepository.FindByPrimaryKey(imageId);
                if (isExistingImage != null)
                {
                    var imagePath = isExistingImage.FilePath;
                    return PhysicalFile(imagePath, "image/jpeg");
                }
                return StatusCode(StatusCodes.Status400BadRequest);
            }
            catch (EntityNotFoundException)
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("GetImageByAlbum")]
        public async Task<IActionResult> GetImagesByAlbum(int albumId)
        {
            var images = new List<ImageDTO>();
            var imageList = await this.UnitOfWork.ImageRepository.FindByAlbumId(albumId);
            foreach (var image in imageList)
            {
                var imagePath = image.FilePath;
                byte[] bytes = System.IO.File.ReadAllBytes(imagePath);
                images.Add(new ImageDTO
                {
                    Name = Path.GetFileName(imagePath),
                    Data = Convert.ToBase64String(bytes, 0, bytes.Length)
                });
            }
            return Ok(images);

            //var imageBytes = new List<byte[]>();
            //var imageList = await this.UnitOfWork.ImageRepository.FindByAlbumId(albumId);
            //foreach (var image in imageList)
            //{
            //    var imagePath = image.FilePath;
            //    byte[] bytes = System.IO.File.ReadAllBytes(imagePath);
            //    imageBytes.Add(bytes);
            //}
            //return Ok(imageBytes);

            //try
            //{
            //    var imageList = await this.UnitOfWork.ImageRepository.FindByAlbumId(albumId);
            //    var physicalFiles = new List<PhysicalFileResult>();
            //    foreach(var image in imageList)
            //    {
            //        var imagePath = image.FilePath;
            //        physicalFiles.Add(PhysicalFile(imagePath,"image/jpeg"));
            //    }
            //    return Ok(physicalFiles);
            //}
            //catch (Exception)
            //{
            //    return StatusCode(StatusCodes.Status500InternalServerError);
            //}
        }

        [HttpPost("UploadImages")]
        public async Task<IActionResult> UploadImages([FromForm] List<IFormFile> postedImages, [FromQuery ]int albumId)
        {
            string path = Path.Combine(this.Environment.WebRootPath, "Uploads");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            var uploadedImages = new List<string>();
            foreach (IFormFile postedImage in postedImages)
            {
                string fileName = Path.GetFileName(postedImage.FileName);
                using (FileStream stream = new FileStream(Path.Combine(path, fileName), FileMode.Create))
                {
                    await postedImage.CopyToAsync(stream);
                    uploadedImages.Add(fileName);
                }

                var image = new Image()
                {
                    FilePath = Path.Combine(path, fileName),
                    Active = true
                };
                try
                {
                    var isExistingAlbum = await this.UnitOfWork.AlbumRepository.FindByPrimaryKey(albumId);
                    if (isExistingAlbum is not null) image.AlbumId = isExistingAlbum.Id;
                }
                catch (EntityNotFoundException)
                {
                    return StatusCode(StatusCodes.Status400BadRequest);
                }
                catch (Exception)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }
                await this.UnitOfWork.ImageRepository.Insert(image);
            }
            await this.UnitOfWork.CommitAsync();
            return Ok(uploadedImages);
        }

        [HttpPost("CreateAlbum")]
        public async Task<IActionResult> CreateAlbum([FromBody] CreateAlbumDTO postedAlbum)
        {
            var album = new Album()
            {
                UserAccountId = postedAlbum.UserAccountId,
                Title = postedAlbum.Title,
                Description = postedAlbum.Description
            };

            await this.UnitOfWork.AlbumRepository.Insert(album);
            await this.UnitOfWork.CommitAsync();
            return Ok(album);
        }
    }
}
