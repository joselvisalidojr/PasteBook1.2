using Microsoft.EntityFrameworkCore;
using PasteBook.Data.Exceptions;
using PasteBook.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasteBook.Data.Repositories
{
    public interface IUserAccountRepository : IBaseRepository<UserAccount>
    {
        UserAccount SoftDelete(UserAccount userAccount);
        UserAccount CheckUserAccount(string userName, string passWord);
    }
    public class UserAccountRepository : GenericRepository<UserAccount>, IUserAccountRepository
    {
        public UserAccountRepository(PasteBookDbContext context) : base(context)
        {
        }

        public UserAccount SoftDelete(UserAccount userAccount)
        {
            userAccount.Active = false;
            this.Context.Attach(userAccount);
            this.Context.Entry<UserAccount>(userAccount).State = EntityState.Modified;
            return userAccount;
        }

        public UserAccount CheckUserAccount(string userName, string passWord)
        {
            UserAccount user = this.Context.UserAccounts.Where(x => x.EmailAddress == userName).FirstOrDefault();
            if (user != null)
            {
                return user;
            }
            return null;
        }
        public bool CheckExistingEmail(string emailAddress)
        {
            UserAccount user = this.Context.UserAccounts.Where(x => x.EmailAddress == emailAddress).FirstOrDefault();
            if (user != null)
            {
                return true;
            }
            return false;
        }
    }
}
