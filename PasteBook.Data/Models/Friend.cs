using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace PasteBook.Data.Models
{
    public partial class Friend : BaseEntity
    {
        public int UserAccountId { get; set; }
        public int FriendAccountId { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [Column(TypeName = "datetime2(7)")]
        public DateTime AddedDate { get; set; }

        [ForeignKey(nameof(UserAccountId))]
        [InverseProperty("Friends")]
        public virtual UserAccount UserAccount { get; set; }

        //[ForeignKey(nameof(FriendAccountId))]
        //[InverseProperty("Friends")]
        //public virtual UserAccount FriendAccount { get; set; }
    }
}
