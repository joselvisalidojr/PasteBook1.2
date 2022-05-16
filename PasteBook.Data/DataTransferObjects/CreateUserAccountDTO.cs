﻿using System;
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
        public string EmailAddress { get; set; }
        [Required]
        [MinLength(1, ErrorMessage = "Password cannot be blank")]
        [MaxLength(30, ErrorMessage = "Password cannot not exceed 30 characters")]
        public string Password { get; set; }
        [Required]
        public DateTime Birthday { get; set; }
        [MinLength(1)]
        [MaxLength(10)]
        public string Gender { get; set; }
        public string? MobileNumber { get; set; }
    }
}
