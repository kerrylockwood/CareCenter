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
        public int IsleNumber { get; set; }

        [Required]
        public int MaxAllowed { get; set; }

        [Required]
        public int PointCost { get; set; }
    }
}
