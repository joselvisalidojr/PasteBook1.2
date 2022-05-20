using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace PasteBook.Data.Models
{
    public partial class FriendRequest : BaseEntity
    {
        public FriendRequest()
        {
            Notifications = new HashSet<Notification>();
        }

        public int RequestReceiverId { get; set; }
        public int RequestSenderId { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [Column(TypeName = "datetime2(7)")]
        public DateTime? RequestDate { get; set; }

        [ForeignKey(nameof(RequestReceiverId))]
        [InverseProperty(nameof(UserAccount.FriendRequests))]
        public virtual UserAccount RequestReceiver { get; set; }

        //[ForeignKey(nameof(RequestSenderId))]
        //[InverseProperty(nameof(UserAccount.FriendRequests))]
        //public virtual UserAccount RequestSender { get; set; }


        [InverseProperty(nameof(Notification.FriendRequest))]
        public virtual ICollection<Notification> Notifications { get; set; }
    }
}
