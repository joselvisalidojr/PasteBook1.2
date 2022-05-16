using PasteBook.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasteBook.Data.Repositories
{
    public interface IImageRepository : IBaseRepository<Image>
    {

    }
    public class ImageRepository : GenericRepository<Image>, IImageRepository
    {
        public ImageRepository(PasteBookDbContext context) : base(context)
        {
        }
    }
}
