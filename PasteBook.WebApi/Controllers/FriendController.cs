using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PasteBook.Data;
using PasteBook.Data.DataTransferObjects;
using PasteBook.Data.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PasteBook.WebApi.Controllers
{
    [Route("Friends")]
    [ApiController]
    [Authorize]
    public class FriendController : ControllerBase
    {
        private readonly IUnitOfWork UnitOfWork;

        public FriendController(IUnitOfWork unitOfWork)
        {
            this.UnitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> GetFriends(int id)
        {
            var friendListData = await this.UnitOfWork.FriendRepository.FindByUserAccountId(id);

            if(friendListData != null)
            {
                var friendListDTO = new List<FriendListDTO>();
                foreach (var friend in friendListData)
                {
                    var FriendAccount = await UnitOfWork.UserAccountRepository.FindByPrimaryKey(friend.FriendAccountId);
                    friendListDTO.Add(new FriendListDTO
                    {
                        Id = friend.Id,
                        UserAccountId = FriendAccount.Id,
                        FirstName = FriendAccount.FirstName,
                        LastName = FriendAccount.LastName,
                        EmailAddress = FriendAccount.EmailAddress,
                        UserName = FriendAccount.UserName,
                        Birthday = FriendAccount.Birthday,
                        Gender = FriendAccount.Gender,
                        AboutMe = FriendAccount.AboutMe,
                        ProfileImagePath = FriendAccount.ProfileImagePath,
                        CoverImagePath = FriendAccount.CoverImagePath,
                        Active = FriendAccount.Active,
                        CreatedDate = FriendAccount.CreatedDate,
                        AddedDate = friend.AddedDate
                    });
                }
                return Ok(friendListDTO);
            }
            return BadRequest();
        }

        [HttpPost("FriendRequest")]
        public async Task<IActionResult> FriendRequest([FromBody] FriendRequestDTO FriendRequest)
        {
            var friendRequest = new FriendRequest()
            {
                RequestReceiverId = FriendRequest.RequestReceiverId,
                RequestSenderId = FriendRequest.RequestSenderId

            };

            await this.UnitOfWork.FriendRequestRepository.Insert(friendRequest);
            await this.UnitOfWork.CommitAsync();
            return Ok(friendRequest);
        }
        [HttpDelete("DeclineFriendRequest")]
        public async Task<IActionResult> DeclineFriendRequest(int id)
        {
            var friendRequest = await this.UnitOfWork.FriendRequestRepository.Delete(id);
            await this.UnitOfWork.CommitAsync();
            return Ok(friendRequest);
        }


        [HttpPost("AcceptFriendRequest")]
        public async Task<IActionResult> AcceptFriendRequest(int id)
        {
            var friendRequest = await this.UnitOfWork.FriendRequestRepository.Delete(id);

            var newFriend = new Friend()
            {
                UserAccountId = friendRequest.RequestReceiverId,
                FriendAccountId = friendRequest.RequestSenderId
            };

            var addFriend = await this.UnitOfWork.FriendRepository.Insert(newFriend);
            await this.UnitOfWork.CommitAsync();
            return Ok(addFriend);
        }
    }
}
