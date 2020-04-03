using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareModels.Customers
{
    public class CustList
    {
        [Key]
        public int CustomerId { get; set; }

        [Display(Name = "Bar Code")]
        public int BarCodeNumber { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        [Display(Name = "Last Name")]
        public int ZipCode { get; set; }

        public string Phone { get; set; }
    }
}
