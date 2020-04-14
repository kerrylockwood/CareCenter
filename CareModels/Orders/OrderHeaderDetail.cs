using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareModels.Orders
{
    public class OrderHeaderDetail
    {
        [Key]
        public int OrderId { get; set; }
        public int CustId { get; set; }
        public int SlotId { get; set; }
        public bool IsCust { get; set; }
        public bool IsPull { get; set; }

        [Display(Name = "Pickup Date/Time")]
        public DateTime? SlotDateTime { get; set; }

        [Display(Name = "First Name")]
        public string CustFirstName { get; set; }

        [Display(Name = "Last Name")]
        public string CustLastName { get; set; }

        [Display(Name = "Delivery")]
        public bool Deliver { get; set; }

        [Display(Name = "Pulled Started At")]
        public DateTimeOffset? PullStartedAt { get; set; }

        [Display(Name = "Pulled Started By")]
        public Guid? PullStartedBy { get; set; }

        [Display(Name = "Pulled By")]
        public string PullStartedName { get; set; }

        [Display(Name = "Order Completed At")]
        public DateTimeOffset? OrderCompletedAt { get; set; }

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

        [Display(Name = "Order Completed")]
        public bool PullCompleted { get; set; }

        [Display(Name = "Created By")]
        public string CreateName { get; set; }

        [Display(Name = "Created At")]
        public DateTimeOffset CreateDateTime { get; set; }

        public List<OrderDetailCategory> OrderDetailCategoryList { get; set; }

    }
}
