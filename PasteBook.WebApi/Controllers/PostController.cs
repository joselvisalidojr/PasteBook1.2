using Microsoft.AspNetCore.Mvc;
using PasteBook.Data;
using PasteBook.Data.DataTransferObjects;
using PasteBook.Data.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PasteBook.WebApi.Controllers
{
    [Route("posts")]
    [ApiController]
    public class PostController: ControllerBase
    {
        private readonly IUnitOfWork UnitOfWork;
        public PostController(IUnitOfWork unitOfWork)
        {
            this.UnitOfWork = unitOfWork;
        }

        [HttpGet("get-posts")]
        public async Task<IActionResult> GetPosts(int id)
        {
            var posts = await this.UnitOfWork.PostRepository.FindAll();

            if (posts != null)
            {
                var postDTO = new List<PostDTO>();
                foreach (var post in posts)
                {
                    var FriendAccount = await UnitOfWork.UserAccountRepository.FindByPrimaryKey(post.UserAccountId);
                    postDTO.Add(new PostDTO
                    {
                        Id = post.Id,
                        UserAccountId = post.UserAccountId,

                        FirstName = FriendAccount.FirstName,
                        LastName = FriendAccount.LastName,
                        EmailAddress = FriendAccount.EmailAddress,
                        UserName = FriendAccount.UserName,
                        Birthday = FriendAccount.Birthday,
                        Gender = FriendAccount.Gender,
                        Active = FriendAccount.Active,
                        AboutMe = FriendAccount.AboutMe,
                        ProfileImagePath = FriendAccount.ProfileImagePath,
                        CoverImagePath = FriendAccount.CoverImagePath,
                        AccountCreatedDate = FriendAccount.CreatedDate,

                        Visibility = post.Visibility,
                        TextContent = post.TextContent,
                        PostCreatedDate = post.CreatedDate,
                        AlbumId = post.AlbumId
                    });
                }
                return Ok(postDTO);
            }
            return BadRequest();
        }
    }
}
