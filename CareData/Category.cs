using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareData
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }

        [Required]
        public string CategoryName { get; set; }

        [ForeignKey(nameof(User))]
        public string CreateBy { get; set; }
        public virtual ApplicationUser User { get; set; }

        [Required]
        public DateTimeOffset CreateAt { get; set; }
    }
}
