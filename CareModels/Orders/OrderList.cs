using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareModels.Orders
{
    public class OrderList
    {
        [Key]
        public int OrderId { get; set; }
        public int CustId { get; set; }
        public int SlotId { get; set; }

        [Display(Name = "Date/Time")]
        public DateTime SlotDateTime { get; set; }

        [Display(Name = "First Name")]
        public string CustFirstName { get; set; }

        [Display(Name = "Last Name")]
        public string CustLastName { get; set; }

        [Display(Name = "Delivery")]
        public bool Deliver { get; set; }

        [Display(Name = "Pull Started")]
        public bool PullStarted { get; set; }

        [Display(Name = "Pull Completed")]
        public bool PullCompleted { get; set; }

        public DateTimeOffset CreateDateTime { get; set; }
    }
}
