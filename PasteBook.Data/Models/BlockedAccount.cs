using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace PasteBook.Data.Models
{
    public partial class BlockedAccount : BaseEntity
    {
        public int BlockerAccounttId { get; set; }
        public int BlockedAccountId { get; set; }

        [ForeignKey(nameof(BlockerAccounttId))]
        [InverseProperty("BlockedAccounts")]
        public virtual UserAccount UserAccount { get; set; }

        [Column(TypeName = "datetime2(7)")]
        public DateTime BlockedDate { get; set; }

        //[ForeignKey(nameof(BlockedAccountId))]
        //[InverseProperty("BlockedAccounts")]
        //public virtual UserAccount BlockedUserAccount { get; set; }
    }
}
