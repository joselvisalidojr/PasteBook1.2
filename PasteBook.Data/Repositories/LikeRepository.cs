using Microsoft.EntityFrameworkCore;
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
        Task<IEnumerable<Like>> FindByPostId(int id);
    }
    public class LikeRepository : GenericRepository<Like>, ILikeRepository
    {
        public LikeRepository(PasteBookDbContext context) : base(context)
        {
        }
        public async Task<IEnumerable<Like>> FindByPostId(int id)
        {
            var likes = await this.Context.Likes.Where(x => x.PostId == id).ToListAsync();
            if (likes != null)
            {
                return likes;
            }
            return null;
        }
    }
}
