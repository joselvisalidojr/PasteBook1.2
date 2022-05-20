using Microsoft.AspNetCore.Mvc;
using PasteBook.Data;
using PasteBook.Data.DataTransferObjects;
using PasteBook.Data.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PasteBook.WebApi.Controllers
{
    [Route("BlockedAccounts")]
    [ApiController]
    public class BlockedAccountController : ControllerBase
    {
        private readonly IUnitOfWork UnitOfWork;

        public BlockedAccountController(IUnitOfWork unitOfWork)
        {
            this.UnitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> GetBlockedAccounts(int id)
        {
            var blockedAccounts = await this.UnitOfWork.BlockedAccountRepository.FindByBlockerAccountId(id);

            if (blockedAccounts != null)
            {
                var blockedAccountsDTO = new List<BlockedAccountDTO>();
                foreach (var blockedAccount in blockedAccounts)
                {
                    var BlockedUserAccount = await UnitOfWork.UserAccountRepository.FindByPrimaryKey(blockedAccount.BlockerAccountId);
                    blockedAccountsDTO.Add(new BlockedAccountDTO
                    {
                        Id = blockedAccount.Id,
                        BlockerAccountId = blockedAccount.BlockerAccountId,
                        BlockedAccountId = blockedAccount.BlockedAccountId,
                        FirstName = BlockedUserAccount.FirstName,
                        LastName = BlockedUserAccount.LastName,
                        Active = BlockedUserAccount.Active,
                        ProfileImagePath = BlockedUserAccount.ProfileImagePath,
                        BlockedDate = blockedAccount.BlockedDate
                    });
                }
                return Ok(blockedAccountsDTO);
            }
            return BadRequest();
        }
    }
}
