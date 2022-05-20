using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasteBook.Data.DataTransferObjects
{
    public class FriendListDTO
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
        public DateTime CreatedDate { get; set; }
        public DateTime AddedDate { get; set; }
    }
    public class FriendRequestDTO
    {
        public int RequestReceiverId { get; set; }
        public int RequestSenderId { get; set; }
    }
}
