using Microsoft.AspNetCore.Mvc;
using PasteBook.Data;
using PasteBook.Data.DataTransferObjects;
using PasteBook.Data.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PasteBook.WebApi.Controllers
{
    [Route("Likes")]
    [ApiController]
    public class LikeController : ControllerBase
    {
        private readonly IUnitOfWork UnitOfWork;

        public LikeController(IUnitOfWork unitOfWork)
        {
            this.UnitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> GetLikes(int id)
        {
            var likes = await this.UnitOfWork.LikeRepository.FindByPostId(id);

            if (likes != null)
            {
                var likesDTO = new List<LikeDTO>();
                foreach (var like in likes)
                {
                    var LikerAccount = await UnitOfWork.UserAccountRepository.FindByPrimaryKey(like.LikerAccountId);
                    likesDTO.Add(new LikeDTO
                    {
                        Id = like.Id,
                        PostId = like.PostId,
                        LikerAccountId = like.LikerAccountId,
                        FirstName = LikerAccount.FirstName,
                        LastName = LikerAccount.LastName,
                        Active = LikerAccount.Active,
                        ProfileImagePath = LikerAccount.ProfileImagePath
                    });
                }
                return Ok(likesDTO);
            }
            return NotFound(null);
        }
    }
}
