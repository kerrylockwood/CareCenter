using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareModels.Orders
{
    public class OrderUpdate
    {
        [Key]
        public int OrderId { get; set; }
        public int CustId { get; set; }
        public string CustName { get; set; }
        public bool IsCust { get; set; }

        [Display(Name = "Pickup Date/Time")]
        public int SlotId { get; set; }

        [Display(Name = "Pickup Date/Time")]
        public DateTime? SlotDateTime { get; set; }

        [Display(Name = "Delivery")]
        public bool Deliver { get; set; }

        [Display(Name = "Most Needed Notes")]
        [MaxLength(1000, ErrorMessage = "Please limit your comment to 1000 characters")]
        public string MostWantedNotes { get; set; }

        [Display(Name = "Fridge and Freezer Notes")]
        [MaxLength(1000, ErrorMessage = "Please limit your comment to 1000 characters")]
        public string FreezerNotes { get; set; }

        [Display(Name = "Fresh Fruits and Vegetable Notes")]
        [MaxLength(1000, ErrorMessage = "Please limit your comment to 1000 characters")]
        public string ProduceNotes { get; set; }

        [Display(Name = "Non Food Notes")]
        [MaxLength(1000, ErrorMessage = "Please limit your comment to 1000 characters")]
        public string NonFoodNotes { get; set; }

        public List<OrderDetailCategory> OrderDetailCategoryList { get; set; }
    }
}
