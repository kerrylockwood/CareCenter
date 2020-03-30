using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareModels.BarCodes
{
    public class BarCodeDetail
    {
        [Key]
        public int BarCodeId { get; set; }

        [Required]
        [Display(Name = "BarCode")]
        public int BarCodeNumber { get; set; }

        [Required]
        [Display(Name ="Created By")]
        public Guid CreateBy { get; set; }

        [Required]
        //[DisplayFormat(DateTime)]
        public DateTimeOffset CreateAt { get; set; }
    }
}
