using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasteBook.Data.Models
{
    public class PostEditHistory : BaseEntity
    {
        public int PostId { get; set; }
        public bool Visibility { get; set; }
        public string Content { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [Column(TypeName = "datetime2(7)")]
        public DateTime UpdatedTimeStamp { get; set; }

        [ForeignKey(nameof(PostId))]
        [InverseProperty("PostEditHistory")]
        public virtual Post Post { get; set; }
    }
}
