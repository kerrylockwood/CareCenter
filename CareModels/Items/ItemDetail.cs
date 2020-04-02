using CareData;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareModels.Items
{
    public class ItemDetail
    {
        [Key]
        public int ItemId { get; set; }

        public int SubCatId { get; set; }

        [Display(Name = "Category")]
        public string CategoryName { get; set; }

        [Display(Name = "SubCategory")]
        public string SubCatName { get; set; }

        [Display(Name = "Item")]
        public string ItemName { get; set; }

        [Display(Name = "Isle Number")]
        public int IsleNumber { get; set; }

        [Display(Name = "Maximum Quantity of Item")]
        public int MaxAllowed { get; set; }

        [Display(Name = "Points per Item")]
        public double PointCost { get; set; }

        [Display(Name = "Created By")]
        public string CreateName { get; set; }

        [Display(Name = "Created At")]
        public DateTimeOffset CreateAt { get; set; }
    }
}
