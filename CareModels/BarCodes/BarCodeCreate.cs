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
        [Display(Name = "BarCode")]
        public int BarCodeNumber { get; set; }

        [Required]
        public Guid CreateBy { get; set; }

        [Required]
        public DateTimeOffset CreateAt { get; set; }
    }
}
