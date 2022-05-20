using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasteBook.Data.DataTransferObjects
{
    public class BlockedAccountDTO
    {
        public int Id { get; set; }
        public int BlockerAccountId { get; set; }
        public int BlockedAccountId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool Active { get; set; }
        public string? ProfileImagePath { get; set; }
        public DateTime BlockedDate { get; set; }
    }
}
