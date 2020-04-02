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

        [Required]
        public int BarCodeId { get; set; }
        [ForeignKey(nameof(BarCodeId))]
        public virtual BarCode BarCode { get; set; }

        [Required]
        [MinLength(2, ErrorMessage ="First Name must contain at least 2 characters")]
        [MaxLength(20, ErrorMessage ="First Name can only be 20 characters long")]
        public string FirstName { get; set; }

        [Required]
        [MinLength(2, ErrorMessage = "Last Name must contain at least 2 characters")]
        [MaxLength(50, ErrorMessage = "Last Name can only be 50 characters long")]
        public string LastName { get; set; }

        [Required]
        [MinLength(2, ErrorMessage = "Address must contain at least 2 characters")]
        [MaxLength(50, ErrorMessage = "Address can only be 50 characters long")]
        public string Address { get; set; }

        [Required]
        [MinLength(2, ErrorMessage = "City must contain at least 2 characters")]
        [MaxLength(50, ErrorMessage = "City can only be 50 characters long")]
        public string City { get; set; }

        [Required]
        [MinLength(2, ErrorMessage = "Please use 2 character State abbreviation")]
        [MaxLength(2, ErrorMessage = "Please use 2 character State abbreviation")]
        public string State { get; set; }

        [Required]
        [Range(0,99999)]
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

        [ForeignKey(nameof(User))]
        public string CreateBy { get; set; }
        public virtual ApplicationUser User { get; set; }

        [Required]
        public DateTimeOffset CreateAt { get; set; }
    }
}
