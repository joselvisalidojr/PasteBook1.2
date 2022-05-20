
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasteBook.Data.DataTransferObjects
{
    public class ImageDTO
    {
        public int Id { get; set; }
        public int AlbumId { get; set; }
        public DateTime UploadedDate { get; set; }
        public string Name { get; set; }
        public string Data { get; set; }
    }
}
