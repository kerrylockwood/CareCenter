using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareModels.BarCodes
{
    public class BarCodeCreate
    {
        [Required]
        [Range(1, 999999999)]
        [Display(Name = "BarCode")]
        public int BarCodeNumber { get; set; }
    }
}
