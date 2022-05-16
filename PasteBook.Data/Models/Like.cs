using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace PasteBook.Data.Models
{
    public partial class Like : BaseEntity
    {
        public Like()
        {
            Notifications = new HashSet<Notification>();
        }

        public int PostId { get; set; }
        public int LikerAccountId { get; set; }

        [ForeignKey(nameof(PostId))]
        [InverseProperty("Likes")]
        public virtual Post Post { get; set; }

        //[ForeignKey(nameof(LikerAccountId))]
        //[InverseProperty("Likes")]
        //public virtual UserAccount LikerAccount { get; set; }

        [InverseProperty(nameof(Notification.Likes))]
        public virtual ICollection<Notification> Notifications { get; set; }
    }
}
