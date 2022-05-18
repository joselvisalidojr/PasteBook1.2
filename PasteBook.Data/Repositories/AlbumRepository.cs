using Microsoft.EntityFrameworkCore;
using PasteBook.Data.DataTransferObjects;
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
        Task<IEnumerable<Album>> FindAlbumId(int userAccountId);
    }
    public class AlbumRepository : GenericRepository<Album>, IAlbumRepository
    {
        public AlbumRepository(PasteBookDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Album>> FindAlbumId(int userAccountId)
        {
            var albums = await this.Context.Albums.Where(x => x.UserAccountId == userAccountId).ToListAsync();
            return albums;
        }
    }
}
