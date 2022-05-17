using Microsoft.AspNetCore.Mvc;
using PasteBook.Data.Models;
using PasteBook.WebApi.Services;
using System.Collections.Generic;
using PasteBook.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using PasteBook.Data.Exceptions;
using System;
using PasteBook.Data.DataTransferObjects;
using Microsoft.AspNetCore.Identity;

namespace PasteBook.WebApi.Controllers
{
    [Route("UserAccount")]
    [ApiController]
    public class UserAccountController : ControllerBase
    {

        private readonly IUserAccountService UserAccountService;
        private readonly IUnitOfWork UnitOfWork;

        public UserAccountController(IUserAccountService userAccountService, IUnitOfWork unitOfWork)
        {
            this.UserAccountService = userAccountService;
            this.UnitOfWork = unitOfWork;
        }
        [HttpGet]
        public IEnumerable<UserAccount> UserAccounts()
        {
            //return UserAccountService.GetAllUser();
            return UnitOfWork.UserAccountRepository.Context.UserAccounts;
        }

        [HttpGet("GetUserAccounts")]
        public async Task<IActionResult> GetUserAccounts()
        {
            var userAccounts = await UnitOfWork.UserAccountRepository.FindAll();
            return Ok(userAccounts);
        }

        [HttpGet("GetUserAccount")]
        public async Task<IActionResult> GetUserAccount(int id)
        {
            try
            {
                var userAccount = await UnitOfWork.UserAccountRepository.FindByPrimaryKey(id);
                return Ok(userAccount);
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

        // must add email verification in this method
        // front end must check password match with confirm password before calling this API
        // email address must be unique (must add email address as unique constraint in UserAccount entity)
        [HttpPost("PostUserAccount")]
        public async Task<IActionResult> PostUserAccount([FromForm] CreateUserAccountDTO userAccount)
        {
            if (ModelState.IsValid)
            {
                var passwordHasherOptions = new PasswordHasherOptions();
                passwordHasherOptions.CompatibilityMode = PasswordHasherCompatibilityMode.IdentityV3;
                passwordHasherOptions.IterationCount = 10_000;
                var passwordHasher = new PasswordHasher<UserAccount>();

                try
                {
                    var newUserAccount = new UserAccount()
                    {
                        FirstName = userAccount.FirstName,
                        LastName = userAccount.LastName,
                        UserName = "initial create",
                        EmailAddress = userAccount.EmailAddress,
                        Birthday = Convert.ToDateTime(userAccount.Birthday + " 00:00:00.0000000"),
                        Gender = userAccount.Gender,
                        MobileNumber = userAccount.MobileNumber,
                        Active = true
                    };

                    var hashedPassword = passwordHasher.HashPassword(newUserAccount, userAccount.Password);
                    newUserAccount.Password = hashedPassword;
                    await UnitOfWork.UserAccountRepository.Insert(newUserAccount);
                    await UnitOfWork.CommitAsync();

                    var userName = $"{newUserAccount.FirstName}{newUserAccount.LastName}{newUserAccount.Id}";
                    newUserAccount.UserName = userName.ToLower();
                    UnitOfWork.UserAccountRepository.Update(newUserAccount);

                    var timelinePhotosAlbum = new Album()
                    {
                        UserAccount = newUserAccount,
                        UserAccountId = newUserAccount.Id,
                        Title = "Timeline photos",
                        Description = ""
                    };

                    var profilePicturesAlbum = new Album()
                    {
                        UserAccount = newUserAccount,
                        UserAccountId = newUserAccount.Id,
                        Title = "Profile pictures",
                        Description = ""
                    };

                    var coverPhotosAlbum = new Album()
                    {
                        UserAccount = newUserAccount,
                        UserAccountId = newUserAccount.Id,
                        Title = "Cover photos",
                        Description = ""
                    };

                    await UnitOfWork.AlbumRepository.Insert(timelinePhotosAlbum);
                    await UnitOfWork.AlbumRepository.Insert(profilePicturesAlbum);
                    await UnitOfWork.AlbumRepository.Insert(coverPhotosAlbum);
                    await UnitOfWork.CommitAsync();

                    // insert request details of FE in DTO here
                    // return StatusCode(StatusCodes.Status201Created, DTO);
                    return StatusCode(StatusCodes.Status201Created);
                }
                catch
                {
                    return StatusCode(StatusCodes.Status400BadRequest);
                }
            }
            return StatusCode(StatusCodes.Status400BadRequest);
        }

        // if user updates email address, check new email address for duplicate within the database
        // must validate new email address with email verification
        // might be better to move change password to another method call
        [HttpPut("UpdateUserAccount")]
        public async Task<IActionResult> UpdateUserAccount(int id, [FromForm] UpdateUserAccountDTO userAccount)
        {
            try
            {
                var existingUserAccount = await UnitOfWork.UserAccountRepository.FindByPrimaryKey(id);

                if (ModelState.IsValid)
                {
                    if (userAccount.FirstName != null)
                    {
                        existingUserAccount.FirstName = userAccount.FirstName;
                    }
                    if (userAccount.LastName != null)
                    {
                        existingUserAccount.LastName = userAccount.LastName;
                    }
                    if (userAccount.EmailAddress != null)
                    {
                        existingUserAccount.EmailAddress = userAccount.EmailAddress;
                    }
                    if (userAccount.Password != null)
                    {
                        existingUserAccount.Password = userAccount.Password;
                    }
                    if (userAccount.Gender != null)
                    {
                        existingUserAccount.Gender = userAccount.Gender;
                    }
                    if (userAccount.MobileNumber != null)
                    {
                        existingUserAccount.MobileNumber = userAccount.MobileNumber;
                    }

                    UnitOfWork.UserAccountRepository.Update(existingUserAccount);
                    await UnitOfWork.CommitAsync();

                    return Ok(userAccount);
                }
                return StatusCode(StatusCodes.Status400BadRequest);
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

        [HttpPut("DeactivateUserAccount")]
        public async Task<IActionResult> DeactivateUserAccount(int id, string password)
        {
            try
            {
                var userAccount = await UnitOfWork.UserAccountRepository.FindByPrimaryKey(id);

                // do verification with password against hashed password in database
                // if password != hashed password { return BadRequest() }

                UnitOfWork.UserAccountRepository.SoftDelete(userAccount);
                await UnitOfWork.CommitAsync();
                return Ok(userAccount);
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

        // for testing only
        [HttpGet("LoginUserAccount")]
        public IActionResult LoginUserAccount(string emailAddress, string password)
        {
            try
            {
                var existingUserAccount = UnitOfWork.UserAccountRepository.FindByEmailAddress(emailAddress);

                var passwordHasherOptions = new PasswordHasherOptions();
                passwordHasherOptions.CompatibilityMode = PasswordHasherCompatibilityMode.IdentityV3;
                passwordHasherOptions.IterationCount = 10_000;
                var passwordHasher = new PasswordHasher<UserAccount>();

                bool verified = false;
                var verificationResult = passwordHasher.VerifyHashedPassword(existingUserAccount, existingUserAccount.Password, password);

                if (verificationResult == PasswordVerificationResult.Success)
                {
                    verified = true;
                }
                if (verificationResult == PasswordVerificationResult.SuccessRehashNeeded)
                {
                    verified = true;
                }
                if (verificationResult == PasswordVerificationResult.Failed)
                {
                    verified = false;
                }
                if (verified == true)
                {
                    return Ok(existingUserAccount);
                }
                if (verified == false)
                {
                    return StatusCode(StatusCodes.Status403Forbidden);
                }
                return StatusCode(StatusCodes.Status400BadRequest);
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
    }
}
