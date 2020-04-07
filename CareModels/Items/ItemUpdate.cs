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
    public class ItemUpdate
    {
        [Key]
        public int ItemId { get; set; }

        [Display(Name = "Sub-Category")]
        public int SubCatId { get; set; }
        [ForeignKey(nameof(SubCatId))]
        public virtual SubCategory SubCategory { get; set; }

        [Display(Name = "Item")]
        public string ItemName { get; set; }

        [Display(Name = "Aisle Number")]
        public int AisleNumber { get; set; }

        [Display(Name = "Maximum Quantity of Item")]
        public int MaxAllowed { get; set; }

        [Display(Name = "Points per Item")]
        public double PointCost { get; set; }
    }
}
