using PasteBook.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasteBook.Data.Repositories
{
    public interface IFriendRepository : IBaseRepository<Friend>
    {

    }
    public class FriendRepository : GenericRepository<Friend>, IFriendRepository
    {
        public FriendRepository(PasteBookDbContext context) : base(context)
        {
        }
    }
}
