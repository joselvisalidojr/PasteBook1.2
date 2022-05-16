using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace PasteBook.Data.Models
{
    public partial class Notification : BaseEntity
    {
        public int UserAccountId { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [Column(TypeName = "datetime2(7)")]
        public DateTime CreatedDate { get; set; }
        [Column(TypeName = "nvarchar(13)")]
        [MinLength(4)]
        [MaxLength(13)]
        public string NotificationType { get; set; }
        public bool? Read { get; set; }
        public int? FriendRequestId { get; set; }
        public int? LikesId { get; set; }
        public int? CommentId { get; set; }

        [ForeignKey(nameof(CommentId))]
        [InverseProperty("Notifications")]
        public virtual Comment Comment { get; set; }
        [ForeignKey(nameof(FriendRequestId))]
        [InverseProperty("Notifications")]
        public virtual FriendRequest FriendRequest { get; set; }
        [ForeignKey(nameof(LikesId))]
        [InverseProperty(nameof(Like.Notifications))]
        public virtual Like Likes { get; set; }

        [ForeignKey(nameof(UserAccountId))]
        [InverseProperty("Notifications")]
        public virtual UserAccount UserAccount { get; set; }
    }
}
