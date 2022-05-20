using Microsoft.EntityFrameworkCore;
using PasteBook.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasteBook.Data.Repositories
{
    public interface ICommentRepository : IBaseRepository<Comment>
    {
        Task<IEnumerable<Comment>> FindByPostId(int id);
    }
    public class CommentRepository : GenericRepository<Comment>, ICommentRepository
    {
        public CommentRepository(PasteBookDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Comment>> FindByPostId(int id)
        {
            var comments = await this.Context.Comments.Where(x => x.PostId == id).ToListAsync();
            if (comments != null)
            {
                return comments;
            }
            return null;
        }
    }
}
