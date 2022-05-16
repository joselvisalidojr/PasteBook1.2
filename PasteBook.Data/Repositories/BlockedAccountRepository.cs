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

    }
    public class BlockedAccountRepository : GenericRepository<BlockedAccount>, IBlockedAccountRepository
    {
        public BlockedAccountRepository(PasteBookDbContext context) : base(context)
        {
        }
    }
}
