using PasteBook.Data.Models;
using System.Collections.Generic;
using System.Linq;
using PasteBook.Data;

namespace PasteBook.WebApi.Services
{
    public interface IUserAccountService
    {
        public IEnumerable<UserAccount> GetAllUser();
    }
    public class UserAccountService: IUserAccountService
    {
        private IUnitOfWork unitOfWork;

        public UserAccountService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public IEnumerable<UserAccount> GetAllUser()
        {
            var userAccounts = unitOfWork.UserAccountRepository.Context.UserAccounts;
            return userAccounts;
        }
    }
}
