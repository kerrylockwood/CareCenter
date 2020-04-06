using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareModels.TimeSlots
{
    public class SlotList
    {
        [Key]
        public int SlotId { get; set; }
        public int DayOfWeekNum { get; set; }

        [Display(Name = "Day Of Week")]
        public string DayOfWeekStr { get; set; }

        [Display(Name = "Time")]
        public TimeSpan Time { get; set; }

        [Display(Name = "Max Pickups per Slot")]
        public int MaxPerSlot { get; set; }
    }
}
