using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareModels.Customers
{
    public class CustDetail
    {
        [Key]
        public int CustomerId { get; set; }

        public int? BarCodeId { get; set; }

        [Display(Name = "Bar Code")]
        public int BarCodeNumber { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        [Display(Name = "ZIP Code")]
        public int ZipCode { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string Email { get; set; }

        [Display(Name = "Number of Children")]
        public int NumberKids { get; set; }

        [Display(Name = "Created By")]
        public string CreateName { get; set; }

        [Display(Name = "Created At")]
        public DateTimeOffset CreateAt { get; set; }
    }
}
