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
        [MaxLength(1000)]
        public string MostWantedNotes { get; set; }

        [Required]
        [MaxLength(1000)]
        public string FreezerNotes { get; set; }

        [Required]
        [MaxLength(1000)]
        public string ProduceNotes { get; set; }

        [Required]
        [MaxLength(1000)]
        public string NonFoodNotes { get; set; }

        [Required]
        public bool Deliver { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }
        public DateTime? PullStartedAt { get; set; }
        public DateTime? OrderCompletedAt { get; set; }
    }
}
