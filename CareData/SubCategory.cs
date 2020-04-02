using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareData
{
    public class SubCategory
    {
        [Key]
        public int SubCatId { get; set; }

        [Required]
        public int CategoryId { get; set; }
        [ForeignKey(nameof(CategoryId))]
        public virtual Category Category { get; set; }

        [Required]
        public string SubCatName { get; set; }

        [Required]
        [Range(0,99)]
        public int SubCatMaxAllowed { get; set; }

        [Required]
        [ForeignKey(nameof(User))]
        public string CreateBy { get; set; }
        public virtual ApplicationUser User { get; set; }

        [Required]
        public DateTimeOffset CreateAt { get; set; }
    }
}
