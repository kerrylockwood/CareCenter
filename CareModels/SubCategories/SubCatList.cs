using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareModels.SubCategories
{
    public class SubCatList
    {
        public int SubCatId { get; set; }
        public int CategoryId { get; set; }

        [Display(Name = "Category")]
        public string CategoryName { get; set; }

        [Display(Name = "Sub-Category")]
        public string SubCatName { get; set; }

        [Display(Name = "Maximum Items for Sub-Category")]
        public int SubCatMaxAllowed { get; set; }
    }
}
