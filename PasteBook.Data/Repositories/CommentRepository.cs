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

    }
    public class CommentRepository : GenericRepository<Comment>, ICommentRepository
    {
        public CommentRepository(PasteBookDbContext context) : base(context)
        {
        }
    }
}
