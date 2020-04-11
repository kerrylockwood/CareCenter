using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareModels.Orders
{
    public class OrderDetailItem
    {
        [Key]
        public int OrderDetailId { get; set; }

        public int ItemId { get; set; }

        public int SubCatId { get; set; }

        [Display(Name = "Item")]
        public string ItemName { get; set; }

        [Display(Name = "Aisle Number")]
        public int AisleNumber { get; set; }

        [Display(Name = "Maximum Quantity of Item")]
        public int MaxAllowed { get; set; }

        [Display(Name = "Points per Item")]
        public double PointCost { get; set; }

        [Display(Name = "Quantity Requested")]
        public int Quantity { get; set; }
        public int QuantityBefore { get; set; }
    }
}
