using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareModels.TimeSlots
{
    public class SlotUpdate
    {
        [Key]
        public int SlotId { get; set; }

        [Required]
        [Display(Name = "Day Of Week")]
        public int DayOfWeekNum { get; set; }

        [Required]
        [Display(Name = "Time")]
        public TimeSpan Time { get; set; }

        [Required]
        [Display(Name = "Max Pickups per Slot")]
        public int MaxPerSlot { get; set; }
    }
}
