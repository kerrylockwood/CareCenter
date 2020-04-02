using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareModels.Categories
{
    public class CategoryDetail
    {
        [Key]
        public int CategoryId { get; set; }

        [Display(Name = "Category")]
        public string CategoryName { get; set; }

        [Display(Name = "Created At")]
        // Formatting set in GraceCAreCenterOrder/Views/Shared/DisplayTemplates
        public DateTimeOffset CreateAt { get; set; }

        [Display(Name = "Created By")]
        public String CreateName { get; set; }
    }
}
