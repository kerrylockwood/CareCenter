using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareModels.Items
{
    public class ItemListShort
    {
        [Key]
        public int ItemId { get; set; }

        [Display(Name = "Item")]
        public string ItemName { get; set; }

        [Display(Name = "Isle Number")]
        public int IsleNumber { get; set; }

        [Display(Name = "Maximum Quantity of Item")]
        public int MaxAllowed { get; set; }

        [Display(Name = "Points per Item")]
        public double PointCost { get; set; }
    }
}
