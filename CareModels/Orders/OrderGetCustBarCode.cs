using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareModels.Orders
{
    public class OrderGetCustBarCode
    {
        [Required]
        [Range(0, 999999999)]
        [Display(Name = "BarCode")]
        public int BarCodeNumber { get; set; }

        public bool IsCust { get; set; }
    }
}
