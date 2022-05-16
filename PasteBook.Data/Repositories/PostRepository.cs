using PasteBook.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasteBook.Data.Repositories
{
    public interface IPostRepository : IBaseRepository<Post>
    {

    }
    public class PostRepository : GenericRepository<Post>, IPostRepository
    {
        public PostRepository(PasteBookDbContext context) : base(context)
        {
        }
    }
}
