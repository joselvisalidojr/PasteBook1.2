using PasteBook.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasteBook.Data.Repositories
{
    public interface IFriendRequestRepository : IBaseRepository<FriendRequest>
    {

    }
    public class FriendRequestRepository : GenericRepository<FriendRequest>, IFriendRequestRepository
    {
        public FriendRequestRepository(PasteBookDbContext context) : base(context)
        {
        }
    }
}
