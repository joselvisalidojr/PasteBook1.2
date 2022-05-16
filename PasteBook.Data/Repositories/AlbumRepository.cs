using PasteBook.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasteBook.Data.Repositories
{
    public interface IAlbumRepository : IBaseRepository<Album>
    {

    }
    public class AlbumRepository : GenericRepository<Album>, IAlbumRepository
    {
        public AlbumRepository(PasteBookDbContext context) : base(context)
        {
        }
    }
}
