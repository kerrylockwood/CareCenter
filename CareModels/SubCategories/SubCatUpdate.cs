using CareData;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareModels.SubCategories
{
    public class SubCatUpdate
    {
        [Key]
        public int SubCatId { get; set; }

        [Required]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }
        [ForeignKey(nameof(CategoryId))]
        public virtual Category Category { get; set; }

        [Required]
        [Display(Name = "Sub-Category")]
        public string SubCatName { get; set; }

        [Required]
        [Range(0, 99)]
        [Display(Name = "Maximum Items for Sub-Category")]
        public int SubCatMaxAllowed { get; set; }
    }
}
