using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasteBook.Data.DataTransferObjects
{
    public class CreateAlbumDTO
    {
        public int UserAccountId { get; set; }
        public string AlbumTitle { get; set; }
        public string? AlbumDescription { get; set; }
    }
}
