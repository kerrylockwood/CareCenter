using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareData
{
    public class Customer
    {
        [Key]
        public int CustomerId { get; set; }

        public int BarCodeId { get; set; }
        [ForeignKey(nameof(BarCodeId))]
        public virtual BarCode BarCode { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string State { get; set; }

        [Required]
        public int ZipCode { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string email { get; set; }

        [Required]
        [Range(0,5)]
        public int NumberKids { get; set; }

        [Required]
        public DateTimeOffset CreatedAt { get; set; }
    }
}
