using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareData
{
    public class TimeSlot
    {
        [Key]
        public int SlotId { get; set; }

        [Required]
        public int DayOfWeek { get; set; }

        [Required]
        public TimeSpan Time { get; set; }

        [Required]
        public int MaxPerSlot { get; set; }

        [Required]
        [ForeignKey(nameof(User))]
        public string CreateBy { get; set; }
        public virtual ApplicationUser User { get; set; }

        [Required]
        public DateTimeOffset CreateAt { get; set; }
    }
}
