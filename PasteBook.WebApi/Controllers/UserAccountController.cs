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
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;

namespace PasteBook.WebApi.Controllers
{
    [Route("user-accounts")]
    [ApiController]
    public class UserAccountController : ControllerBase
    {
        private readonly IUnitOfWork UnitOfWork;
        private IConfiguration _config;

        public UserAccountController(IUnitOfWork unitOfWork, IConfiguration config)
        {
            this.UnitOfWork = unitOfWork;
            this._config = config;
        }
        [HttpGet]
        public IEnumerable<UserAccount> UserAccounts()
        {
            return UnitOfWork.UserAccountRepository.Context.UserAccounts;
        }

        [HttpGet("get-user-accounts")]
        public async Task<IActionResult> GetUserAccounts()
        {
            var userAccounts = await UnitOfWork.UserAccountRepository.FindAll();
            return Ok(userAccounts);
        }

        [HttpGet("get-user-account")]
        public async Task<IActionResult> GetUserAccount(int userAccountId)
        {
            try
            {
                var existingUserAccount = await UnitOfWork.UserAccountRepository.FindByPrimaryKey(userAccountId);
                var existingUserAccountDTO = new UserAccountDTO()
                {
                    id = existingUserAccount.Id,
                    firstName = existingUserAccount.FirstName,
                    lastName = existingUserAccount.LastName,
                    userName = existingUserAccount.UserName,
                    emailAddress = existingUserAccount.EmailAddress,
                    birthday = existingUserAccount.Birthday,
                    gender = existingUserAccount.Gender ??= "",
                    mobileNumber = existingUserAccount.MobileNumber ??= "",
                    createdDate = existingUserAccount.CreatedDate,
                    profileImagePath = existingUserAccount.ProfileImagePath,
                    coverImagePath = existingUserAccount.CoverImagePath
                };
                return Ok(existingUserAccountDTO);
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

        [HttpPost("create-user-account")]
        public async Task<IActionResult> CreateUserAccount([FromForm] CreateUserAccountDTO userAccount)
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
                        Description = "",
                        Active = true
                    };

                    var profilePicturesAlbum = new Album()
                    {
                        UserAccount = newUserAccount,
                        UserAccountId = newUserAccount.Id,
                        Title = "Profile pictures",
                        Description = "",
                        Active = true
                    };

                    var coverPhotosAlbum = new Album()
                    {
                        UserAccount = newUserAccount,
                        UserAccountId = newUserAccount.Id,
                        Title = "Cover photos",
                        Description = "",
                        Active = true
                    };

                    await UnitOfWork.AlbumRepository.Insert(timelinePhotosAlbum);
                    await UnitOfWork.AlbumRepository.Insert(profilePicturesAlbum);
                    await UnitOfWork.AlbumRepository.Insert(coverPhotosAlbum);
                    await UnitOfWork.CommitAsync();

                    return StatusCode(StatusCodes.Status201Created, true);
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
        [HttpPut("update-user-account")]
        public async Task<IActionResult> UpdateUserAccount(int id, [FromForm] UpdateUserAccountDTO userAccount)
        {
            try
            {
                var existingUserAccount = await UnitOfWork.UserAccountRepository.FindByPrimaryKey(id);

                if (ModelState.IsValid)
                {
                    existingUserAccount.FirstName = userAccount.FirstName ??= existingUserAccount.FirstName;
                    existingUserAccount.LastName = userAccount.LastName ??= existingUserAccount.LastName;
                    existingUserAccount.EmailAddress = userAccount.EmailAddress ??= existingUserAccount.EmailAddress;
                    existingUserAccount.Password = userAccount.Password ??= existingUserAccount.Password;
                    existingUserAccount.Gender = userAccount.Gender ??= existingUserAccount.Gender;
                    existingUserAccount.MobileNumber = userAccount.MobileNumber ??= existingUserAccount.MobileNumber;

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

        [HttpPut("deactivate-user-account")]
        public async Task<IActionResult> DeactivateUserAccount(int id, string password)
        {
            try
            {
                var existingUserAccount = await UnitOfWork.UserAccountRepository.FindByPrimaryKey(id);

                var passwordHasherOptions = new PasswordHasherOptions();
                passwordHasherOptions.CompatibilityMode = PasswordHasherCompatibilityMode.IdentityV3;
                passwordHasherOptions.IterationCount = 10_000;
                var passwordHasher = new PasswordHasher<UserAccount>();

                bool verified = false;
                var verificationResult = passwordHasher.VerifyHashedPassword(existingUserAccount, existingUserAccount.Password, password);

                if (verificationResult == PasswordVerificationResult.Failed)
                {
                    verified = false;
                }
                if (verificationResult == PasswordVerificationResult.Success)
                {
                    verified = true;
                }
                if (verificationResult == PasswordVerificationResult.SuccessRehashNeeded)
                {
                    verified = true;
                }
                if (verified == false)
                {
                    return StatusCode(StatusCodes.Status400BadRequest);
                }
                if (verified == true)
                {
                    UnitOfWork.UserAccountRepository.SoftDelete(existingUserAccount);
                    await UnitOfWork.CommitAsync();
                }
                return Ok(existingUserAccount);
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

        [HttpPost("login-user-account")]
        public IActionResult LoginUserAccount([FromBody] LogInCredentials logInCredentials)
        {
            var emailAddress = logInCredentials.email;
            var password = logInCredentials.password;
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
                    var token = Generate();

                    var logInDetails = new LogInDTO
                    {
                        id = existingUserAccount.Id,
                        email = existingUserAccount.EmailAddress,
                        token = token
                    };
                    return Ok(logInDetails);
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

        private string Generate()
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                expires: DateTime.Now.AddDays(3),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
