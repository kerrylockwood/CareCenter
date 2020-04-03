using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareModels.Customers
{
    public class CustBarCode
    {
        [Key]
        public int BarCodeId { get; set; }

        [Display(Name = "Bar Code")]
        [Range(0, 999999999, ErrorMessage ="BarCode cannot be negative or too big")]
        public int BarCodeNumber { get; set; }
    }
}
