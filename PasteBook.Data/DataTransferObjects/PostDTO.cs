using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasteBook.Data.DataTransferObjects
{
    public class PostDTO
    {
        public int Id { get; set; }
        public int UserAccountId { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string UserName { get; set; }
        public DateTime Birthday { get; set; }
        public string Gender { get; set; }
        public bool Active { get; set; }
        public string AboutMe { get; set; }
        public string? ProfileImagePath { get; set; }
        public string? CoverImagePath { get; set; }
        public DateTime AccountCreatedDate { get; set; }


        public string Visibility { get; set; }
        public string TextContent { get; set; }
        public DateTime PostCreatedDate { get; set; }
        public int? AlbumId { get; set; }
    }
}
