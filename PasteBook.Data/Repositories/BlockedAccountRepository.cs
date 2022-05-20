using Microsoft.EntityFrameworkCore;
using PasteBook.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasteBook.Data.Repositories
{
    public interface IBlockedAccountRepository : IBaseRepository<BlockedAccount>
    {
        Task<IEnumerable<BlockedAccount>> FindByBlockerAccountId(int id);
        Task<IEnumerable<BlockedAccount>> FindByBlockedAccountId(int id);
    }
    public class BlockedAccountRepository : GenericRepository<BlockedAccount>, IBlockedAccountRepository
    {
        public BlockedAccountRepository(PasteBookDbContext context) : base(context)
        {
        }
        public async Task<IEnumerable<BlockedAccount>> FindByBlockerAccountId(int id)
        {
            var blockedAccounts = await this.Context.BlockedAccounts.Where(x => x.BlockerAccountId == id).ToListAsync();
            if (blockedAccounts != null)
            {
                return blockedAccounts;
            }
            return null;
        }
        public async Task<IEnumerable<BlockedAccount>> FindByBlockedAccountId(int id)
        {
            var blockedAccounts = await this.Context.BlockedAccounts.Where(x => x.BlockedAccountId == id).ToListAsync();
            if (blockedAccounts != null)
            {
                return blockedAccounts;
            }
            return null;
        }
    }
}
