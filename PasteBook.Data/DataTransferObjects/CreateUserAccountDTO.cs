using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasteBook.Data.DataTransferObjects
{
    public partial class CreateUserAccountDTO
    {
        [Required]
        [MinLength(1, ErrorMessage = "First name cannot be blank")]
        [MaxLength(50, ErrorMessage = "First name cannot not exceed 50 characters")]
        public string FirstName { get; set; }
        [Required]
        [MinLength(1, ErrorMessage = "Last name cannot be blank")]
        [MaxLength(50, ErrorMessage = "Last name cannot not exceed 50 characters")]
        public string LastName { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string EmailAddress { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        public string Birthday { get; set; }
        [MinLength(1)]
        [MaxLength(10)]
        public string Gender { get; set; }
        public string? MobileNumber { get; set; }
    }
}
