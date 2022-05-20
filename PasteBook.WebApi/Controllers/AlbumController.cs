using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PasteBook.Data;
using PasteBook.Data.DataTransferObjects;
using PasteBook.Data.Exceptions;
using PasteBook.Data.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PasteBook.WebApi.Controllers
{
    [Route("albums")]
    [ApiController]
    public class AlbumController : ControllerBase
    {
        private readonly IUnitOfWork UnitOfWork;
        public AlbumController(IUnitOfWork UnitOfWork)
        {
            this.UnitOfWork = UnitOfWork;
        }

        [HttpPost("create-album")]
        public async Task<IActionResult> CreateAlbum([FromBody] CreateAlbumDTO postedAlbum)
        {
            var album = new Album()
            {
                UserAccountId = postedAlbum.UserAccountId,
                Title = postedAlbum.Title ??= "",
                Description = postedAlbum.Description ??= "",
                Active = true
            };

            await this.UnitOfWork.AlbumRepository.Insert(album);
            await this.UnitOfWork.CommitAsync();
            return Ok(album);
        }

        [HttpGet("get-album")]

        public async Task<IActionResult> GetAlbum(int albumId)
        {
            var album = await this.UnitOfWork.AlbumRepository.FindByPrimaryKey(albumId);
            var albumCoverImage = await this.UnitOfWork.ImageRepository.FindByAlbumCoverPhoto(album.Id);
            string albumCoverImageData = null;
            if (albumCoverImage != null)
            {
                var albumCoverImagePath = albumCoverImage.FilePath;
                byte[] bytes = System.IO.File.ReadAllBytes(albumCoverImagePath);
                albumCoverImageData = Convert.ToBase64String(bytes, 0, bytes.Length);
            }

            var albumDTO = new AlbumDTO()
            {
                Id = album.Id,
                Title = album.Title,
                CoverPhoto = albumCoverImageData,
                Description = album.Description,
                CreatedDate = album.CreationDate
            };
            return Ok(albumDTO);
        }

        [HttpGet("get-albums")]

        public async Task<IActionResult> GetAlbums(int userAccountId)
        {
            var albumList = await this.UnitOfWork.AlbumRepository.FindAlbumId(userAccountId);
            var albumListDTO = new List<AlbumDTO>();
            foreach (var album in albumList)
            {
                var albumCoverImage = await this.UnitOfWork.ImageRepository.FindByAlbumCoverPhoto(album.Id);
                string albumCoverImageData = null;
                if (albumCoverImage != null)
                {
                    var albumCoverImagePath = albumCoverImage.FilePath;
                    byte[] bytes = System.IO.File.ReadAllBytes(albumCoverImagePath);
                    albumCoverImageData = Convert.ToBase64String(bytes, 0, bytes.Length);
                }

                albumListDTO.Add(new AlbumDTO
                {
                    Id = album.Id,
                    Title = album.Title,
                    CoverPhoto = albumCoverImageData,
                    Description = album.Description,
                    CreatedDate = album.CreationDate
                });
            }
            return Ok(albumListDTO);
        }

        [HttpPut("delete-album/{albumId=0}")]
        public async Task<IActionResult> DeleteAlbum([FromRoute] int albumId)
        {
            try
            {
                var existingAlbum = await UnitOfWork.AlbumRepository.FindByPrimaryKey(albumId);
                if (existingAlbum.Title == "Timeline photos" || existingAlbum.Title == "Profile pictures" || existingAlbum.Title == "Cover photos")
                {
                    return StatusCode(StatusCodes.Status403Forbidden);
                }
                UnitOfWork.AlbumRepository.SoftDelete(existingAlbum);
                await UnitOfWork.CommitAsync();
                return Ok(existingAlbum);
            }
            catch (EntityNotFoundException)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut("edit-album/{albumId=0}")]
        public async Task<IActionResult> EditAlbum([FromBody] EditAlbumDTO updatedAlbum, [FromRoute] int albumId)
        {
            try
            {
                var existingAlbum = await UnitOfWork.AlbumRepository.FindByPrimaryKey(albumId);
                if (existingAlbum.Title == "Timeline photos" || existingAlbum.Title == "Profile pictures" || existingAlbum.Title == "Cover photos")
                {
                    return StatusCode(StatusCodes.Status403Forbidden);
                }
                existingAlbum.Title = updatedAlbum.Title ??= existingAlbum.Title;
                existingAlbum.Description = updatedAlbum.Description ??= existingAlbum.Description;
                UnitOfWork.AlbumRepository.Update(existingAlbum);
                await UnitOfWork.CommitAsync();
                return Ok(existingAlbum);
            }
            catch (EntityNotFoundException)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            catch (EntityDataException)
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
