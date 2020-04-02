using CareData;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareModels.Items
{
    public class ItemCreate
    {
        [Key]
        public int ItemId { get; set; }

        [Required]
        public int SubCatId { get; set; }
        [ForeignKey(nameof(SubCatId))]
        public virtual SubCategory SubCategory { get; set; }

        [Required]
        [Display(Name = "Item")]
        public string ItemName { get; set; }

        [Required]
        [Display(Name = "Isle Number")]
        public int IsleNumber { get; set; }

        [Required]
        [Display(Name = "Maximum Quantity of Item")]
        public int MaxAllowed { get; set; }

        [Required]
        [Display(Name = "Points per Item")]
        public double PointCost { get; set; }
    }
}
