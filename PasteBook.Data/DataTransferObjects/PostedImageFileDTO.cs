using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasteBook.Data.DataTransferObjects
{
    public class PostedImageFileDTO
    {
        public List<IFormFile> imageFiles { get; set; }
    }
}
