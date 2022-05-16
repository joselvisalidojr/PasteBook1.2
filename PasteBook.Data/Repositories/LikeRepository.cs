using PasteBook.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasteBook.Data.Repositories
{
    public interface ILikeRepository : IBaseRepository<Like>
    {

    }
    public class LikeRepository : GenericRepository<Like>, ILikeRepository
    {
        public LikeRepository(PasteBookDbContext context) : base(context)
        {
        }
    }
}
