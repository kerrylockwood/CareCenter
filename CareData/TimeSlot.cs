﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        public Guid CreateBy { get; set; }

        [Required]
        public DateTimeOffset CreateAt { get; set; }
    }
}
