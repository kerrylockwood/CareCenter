using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareModels.SubCategories
{
    public class CatSubCatList
    {
        public int SubCatId { get; set; }

        [Display(Name = "Catagory | Sub-Category")]
        public string CatSubCatName { get; set; }
    }
}
