using Microsoft.AspNetCore.Mvc;
using PasteBook.Data;
using PasteBook.Data.DataTransferObjects;
using PasteBook.Data.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PasteBook.WebApi.Controllers
{
    [Route("Comments")]
    [ApiController]
    public class CommentController: ControllerBase
    {
        private readonly IUnitOfWork UnitOfWork;
        public CommentController(IUnitOfWork unitOfWork)
        {
            this.UnitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> GetComments(int id)
        {
            var comments = await this.UnitOfWork.CommentRepository.FindByPostId(id);

            if (comments != null)
            {
                var commentListDTO = new List<CommentDTO>();
                foreach (var comment in comments)
                {
                    var CommentingUser = await UnitOfWork.UserAccountRepository.FindByPrimaryKey(comment.CommentingUserId);
                    commentListDTO.Add(new CommentDTO
                    {
                        Id = comment.Id,
                        PostId = comment.PostId,
                        CommentingUserId = comment.CommentingUserId,
                        FirstName = CommentingUser.FirstName,
                        LastName = CommentingUser.LastName,
                        ProfileImagePath = CommentingUser.ProfileImagePath,
                        Active = CommentingUser.Active,
                        CommentContent = comment.CommentContent,
                        CreatedDate = comment.CreatedDate
                    });
                }
                return Ok(commentListDTO);
            }
            return NotFound(null);
        }
    }
}
