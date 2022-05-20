using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasteBook.Data.DataTransferObjects
{
    public partial class LogInDTO
    {
        public int id { get; set; }
        public string email { get; set; }
        public string token { get; set; }
    }

    public partial class LogInCredentials
    {
        public string email { get; set; }
        public string password { get; set; }
    }
}
