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
        Album SoftDelete(Album album);
    }
    public class AlbumRepository : GenericRepository<Album>, IAlbumRepository
    {
        public AlbumRepository(PasteBookDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Album>> FindAlbumId(int userAccountId)
        {
            var albums = await this.Context.Albums.Where(x => (x.UserAccountId == userAccountId) && (x.Active == true)).ToListAsync();
            return albums;
        }

        public Album SoftDelete(Album album)
        {
            album.Active = false;
            this.Context.Attach(album);
            this.Context.Entry<Album>(album).State = EntityState.Modified;
            return album;
        }
    }
}
