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
using System.Threading.Tasks;

namespace PasteBook.WebApi.Controllers
{
    [Route("images")]
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

        [HttpGet("get-image")]
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

        [HttpGet("get-image-by-album")]
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
                    Id = image.Id,
                    AlbumId = image.AlbumId,
                    UploadedDate = image.UploadedDate,
                    Name = Path.GetFileName(imagePath),
                    Data = Convert.ToBase64String(bytes, 0, bytes.Length)
                });
            }
            return Ok(images);
        }

        [HttpPost("upload-image/{albumId=0}")]
        public async Task<IActionResult> UploadImage([FromForm] IFormFile postedImage, [FromRoute] int albumId)
        {
            if(postedImage == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }
            try
            {
                string path = Path.Combine(this.Environment.WebRootPath, "/Uploads");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                var uploadedImage = new List<string>();
                string fileName = Path.GetFileName(postedImage.FileName);
                using (FileStream stream = new FileStream(Path.Combine(path, fileName), FileMode.Create))
                {
                    await postedImage.CopyToAsync(stream);
                    uploadedImage.Add(fileName);
                }

                var image = new Image()
                {
                    FilePath = Path.Combine(path, fileName),
                    Active = true
                };

                var isExistingAlbum = await this.UnitOfWork.AlbumRepository.FindByPrimaryKey(albumId);
                if (isExistingAlbum is not null) image.AlbumId = isExistingAlbum.Id;

                await this.UnitOfWork.ImageRepository.Insert(image);
                await this.UnitOfWork.CommitAsync();
                return Ok(uploadedImage);
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

        [HttpPost("upload-images/{albumId=0}")]
        public async Task<IActionResult> UploadImages([FromBody] PostedImageFileDTO postedImageFile, [FromRoute] int albumId)
        {
            string path = Path.Combine(this.Environment.WebRootPath, "Uploads");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            var uploadedImages = new List<string>();
            foreach (IFormFile imageFile in postedImageFile.imageFiles)
            {
                string fileName = Path.GetFileName(imageFile.FileName);
                using (FileStream stream = new FileStream(Path.Combine(path, fileName), FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
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
    }
}
