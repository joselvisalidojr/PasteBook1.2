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
    [Route("Images")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly IUnitOfWork UnitOfWork;
        public ImageController(IUnitOfWork UnitOfWork)
        {
            this.UnitOfWork = UnitOfWork;
        }

        [HttpPost("UploadImages")]
        public async Task<IActionResult> UploadImages([FromBody] List<PostedImageDTO> imagePaths, [FromQuery ]int albumId)
        {
            var fileNames = new List<string>();
            foreach (PostedImageDTO imagePath in imagePaths)
            {
                var image = new Image()
                {
                    FilePath = imagePath.FilePath,
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
                fileNames.Add(imagePath.FilePath);
            }
            await this.UnitOfWork.CommitAsync();
            return Ok(fileNames);
        }

        [HttpPost("CreateAlbum")]
        public async Task<IActionResult> CreateAlbum([FromQuery] CreateAlbumDTO postedAlbum)
        {
            var album = new Album()
            {
                UserAccountId = postedAlbum.UserAccountId,
                Title = postedAlbum.AlbumTitle,
                Description = postedAlbum.AlbumDescription
            };

            await this.UnitOfWork.AlbumRepository.Insert(album);
            await this.UnitOfWork.CommitAsync();
            return Ok(album);
        }
    }
}
