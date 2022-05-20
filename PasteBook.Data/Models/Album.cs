using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace PasteBook.Data.Models
{
    public partial class Album : BaseEntity
    {
        public Album()
        {
            Images = new HashSet<Image>();
        }

        public int UserAccountId { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        [MinLength(1, ErrorMessage = "Title cannot be blank")]
        [MaxLength(50, ErrorMessage = "Title should not exceed 50 characters")] 
        public string Title { get; set; }

        [Column(TypeName = "nvarchar(Max)")]
        public string Description { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [Column(TypeName = "datetime")]
        public DateTime CreationDate { get; set; }
        public Boolean Active { get; set; }
        public virtual ICollection<Image> Images { get; set; }

        [ForeignKey(nameof(UserAccountId))]
        [InverseProperty("Albums")]
        public virtual UserAccount UserAccount { get; set; }
    }
}
