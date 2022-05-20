using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasteBook.Data.DataTransferObjects
{
    public class UserAccountDTO
    {
        public int id { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string userName { get; set; }
        public string emailAddress { get; set; }
        public DateTime birthday { get; set; }
        public string gender { get; set; }
        public string mobileNumber { get; set; }
        public DateTime createdDate { get; set; }
        public string coverImagePath { get; set; }
        public string profileImagePath { get; set; }
    }
}
