using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasteBook.Data.DataTransferObjects
{
    public partial class UpdateUserAccountDTO
    {
        [MaxLength(50, ErrorMessage = "First name cannot not exceed 50 characters")]
        public string? FirstName { get; set; }
        [MaxLength(50, ErrorMessage = "Last name cannot not exceed 50 characters")]
        public string? LastName { get; set; }
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string? EmailAddress { get; set; }
        [DataType(DataType.Password)]
        public string? Password { get; set; }
        public DateTime? Birthday { get; set; }
        [MaxLength(10)]
        public string? Gender { get; set; }
        public string? MobileNumber { get; set; }
    }
}
