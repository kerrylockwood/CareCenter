using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareData
{
    public class BarCode
    {
        [Key]
        public int BarCodeId { get; set; }

        [Required]
        public int BarCodeNumber { get; set; }
    }
}
