using Microsoft.EntityFrameworkCore;
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
        Task<IEnumerable<Image>> FindByAlbumId(int albumId);
        Task<Image> FindByAlbumCoverPhoto(int albumId);
    }
    public class ImageRepository : GenericRepository<Image>, IImageRepository
    {
        public ImageRepository(PasteBookDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Image>> FindByAlbumId(int albumId)
        {
            var imageList = await this.Context.Images.Where(x => x.AlbumId == albumId).ToListAsync();
            return imageList;
        }
        public async Task<Image> FindByAlbumCoverPhoto(int albumId)
        {
            var image = await this.Context.Images.Where(x => x.AlbumId == albumId).OrderBy(x => x.UploadedDate).FirstOrDefaultAsync();
            if(image is object)
            {
                return image;
            }
            else
            {
                return null;
            }
        }
    }
}
