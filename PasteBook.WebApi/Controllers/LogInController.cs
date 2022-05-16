﻿using Microsoft.AspNetCore.Mvc;
using PasteBook.Data;
using PasteBook.WebApi.Services;
using System.Threading.Tasks;
using PasteBook.Data.DataTransferObjects;
using Microsoft.AspNetCore.Http;

namespace PasteBook.WebApi.Controllers
{
    [Route("UserAccount")]
    [ApiController]
    public class LogInController: ControllerBase
    {

        private readonly IUserAccountService UserAccountService;
        private readonly IUnitOfWork UnitOfWork;

        public LogInController(IUserAccountService userAccountService, IUnitOfWork unitOfWork)
        {
            this.UserAccountService = userAccountService;
            this.UnitOfWork = unitOfWork;
        }

        [HttpPost("LogIn")]
        public async Task<IActionResult> LogIn([FromForm] LogInDTO userAccount)
        {
            if (ModelState.IsValid)
            {
                var user = UnitOfWork.UserAccountRepository.CheckUserAccount(userAccount.UserName, userAccount.PassWord);
                if (user != null)
                {
                    return StatusCode(StatusCodes.Status200OK, user);
                }
                else
                {
                    return StatusCode(StatusCodes.Status403Forbidden);
                }
            }

            return StatusCode(StatusCodes.Status400BadRequest);
        }
    }
}
