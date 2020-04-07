using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareData
{
    public class Item
    {
        [Key]
        public int ItemId { get; set; }

        [Required]
        public int SubCatId { get; set; }
        [ForeignKey(nameof(SubCatId))]
        public virtual SubCategory SubCategory { get; set; }

        [Required]
        public string ItemName { get; set; }

        [Required]
        public int AisleNumber { get; set; }

        [Required]
        public int MaxAllowed { get; set; }

        [Required]
        public double PointCost { get; set; }

        [ForeignKey(nameof(User))]
        public string CreateBy { get; set; }
        public virtual ApplicationUser User { get; set; }

        [Required]
        public DateTimeOffset CreateAt { get; set; }
    }
}
