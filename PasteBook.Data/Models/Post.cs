using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace PasteBook.Data.Models
{
    public partial class Post : BaseEntity
    {
        public Post()
        {
            Albums = new Album();
            Comments = new HashSet<Comment>();
            Likes = new HashSet<Like>();
        }

        public int UserAccountId { get; set; }
        public bool Visibility { get; set; }
        public string TextContent { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [Column(TypeName = "datetime2(7)")]
        public DateTime CreatedDate { get; set; }

        [ForeignKey(nameof(UserAccountId))]
        [InverseProperty("Posts")]
        public virtual UserAccount UserAccount { get; set; }
        public int? AlbumId { get; set; }
        public virtual Album Albums { get; set; }
        [InverseProperty(nameof(Comment.Post))]
        public virtual ICollection<Comment> Comments { get; set; }
        [InverseProperty(nameof(Like.Post))]
        public virtual ICollection<Like> Likes { get; set; }
    }
}
