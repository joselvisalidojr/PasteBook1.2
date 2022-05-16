using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace PasteBook.Data.Models
{
    public partial class Image : BaseEntity
    {
        public int AlbumId { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [Column(TypeName = "datetime2(7)")]
        public DateTime UploadedDate { get; set; }
        public string FilePath { get; set; }
        public bool Active { get; set; }

        [ForeignKey(nameof(AlbumId))]
        [InverseProperty("Images")]
        public virtual Album Album { get; set; }
    }
}
