using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareData
{
    public class OrderHeader
    {
        [Key]
        public int OrderId { get; set; }

        [Required]
        public int CustId { get; set; }
        [ForeignKey(nameof(CustId))]
        public virtual Customer Customer { get; set; }

        [Required]
        public int SlotId { get; set; }
        [ForeignKey(nameof(SlotId))]
        public virtual TimeSlot TimeSlot { get; set; }

        [Required]
        [MaxLength(1000, ErrorMessage ="Please limit your comment to 1000 characters")]
        public string MostWantedNotes { get; set; }

        [Required]
        [MaxLength(1000, ErrorMessage = "Please limit your comment to 1000 characters")]
        public string FreezerNotes { get; set; }

        [Required]
        [MaxLength(1000, ErrorMessage = "Please limit your comment to 1000 characters")]
        public string ProduceNotes { get; set; }

        [Required]
        [MaxLength(1000, ErrorMessage = "Please limit your comment to 1000 characters")]
        public string NonFoodNotes { get; set; }

        [Required]
        public bool Deliver { get; set; }

        [Required]
        public Guid CreateBy { get; set; }

        [Required]
        public DateTimeOffset CreatedAt { get; set; }

        public Guid? PullStartedBy { get; set; }

        public DateTimeOffset? PullStartedAt { get; set; }

        public DateTimeOffset? OrderCompletedAt { get; set; }
    }
}
