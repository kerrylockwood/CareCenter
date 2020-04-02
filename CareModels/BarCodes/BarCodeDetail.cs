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

        [Display(Name = "BarCode")]
        public int BarCodeNumber { get; set; }

        [Display(Name = "Created At")]
        // Formatting set in GraceCAreCenterOrder/Views/Shared/DisplayTemplates
        public DateTimeOffset CreateAt { get; set; }

        [Display(Name = "Created By")]
        public String CreateName { get; set; }
    }
}
