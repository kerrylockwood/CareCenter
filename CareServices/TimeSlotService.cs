using CareData;
using CareModels.TimeSlots;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareServices
{
    public class TimeSlotService
    {
        private readonly string _userId;

        public TimeSlotService(string userId)
        {
            _userId = userId;
        }

        public IEnumerable<SlotList> GetTimeSlots()
        {
            using (var ctx = new ApplicationDbContext())
            {
                var query =
                    ctx
                        .TimeSlots
                        .Select(
                            e =>
                                new SlotList
                                {
                                    SlotId = e.SlotId,
                                    DayOfWeekNum = e.DayOfWeekNum,
                                    DayOfWeekStr = ((DayOfWeek)e.DayOfWeekNum).ToString(),
                                    Time = e.Time,
                                    MaxPerSlot = e.MaxPerSlot,
                                }
                        );

                return query.ToArray();
            }
        }

        public IEnumerable<TimeSelect> GetTimeSlotDropDown(bool delivery)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var query =
                    ctx
                        .TimeSlots
                        .Select(
                            e =>
                                new SlotList
                                {
                                    SlotId = e.SlotId,
                                    DayOfWeekNum = e.DayOfWeekNum,
                                    DayOfWeekStr = ((DayOfWeek)e.DayOfWeekNum).ToString(),
                                    Time = e.Time,
                                    MaxPerSlot = e.MaxPerSlot,
                                }
                        );
                var orderService = new OrderService(_userId);

                List<TimeSelect> timeSelectList = new List<TimeSelect>();

                foreach (SlotList slot in query)
                {
                    DateTime slotTime = orderService.ConvertSlotToDateTime(slot.SlotId, DateTime.Now, false, _userId);
                    if (DateTime.Now.AddMinutes(120) < slotTime)
                    {
                        var lastTimeSlot = GetMaxTimeSlot();
                        orderService.ConvertSlotToDateTime(lastTimeSlot.SlotId, DateTime.Now, delivery,_userId);
                        int slotCount = orderService.GetSlotCount(slot.SlotId, false);
                        if (slotCount < slot.MaxPerSlot)
                        {
                            TimeSelect timeSelect = new TimeSelect
                            {
                                SlotId = slot.SlotId,
                                SlotTime = slotTime
                            };
                            timeSelectList.Add(timeSelect);
                        }
                    }
                }

                return timeSelectList.ToArray();
            }
        }

        public SlotDetail GetTimeSlotById(int id)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity =
                    ctx
                        .TimeSlots
                        .Single(e => e.SlotId == id);
                return
                    new SlotDetail
                    {
                        SlotId = entity.SlotId,
                        DayOfWeekNum = entity.DayOfWeekNum,
                        DayOfWeekStr = ((DayOfWeek)entity.DayOfWeekNum).ToString(),
                        Time = entity.Time,
                        MaxPerSlot = entity.MaxPerSlot,
                        CreateAt = entity.CreateAt,
                        CreateName = entity.User.UserName
                    };
            }
        }

        public SlotDetail GetTimeSlotByDayTime(int day, TimeSpan time)
        {
            using (var ctx = new ApplicationDbContext())
            {
                if (ctx.TimeSlots.Count(e => e.DayOfWeekNum == day && e.Time == time) == 0)
                {
                    return new SlotDetail { SlotId = 0 };
                }
                var entity =
                    ctx
                        .TimeSlots
                        .Single(e => e.DayOfWeekNum == day && e.Time == time);
                return
                    new SlotDetail
                    {
                        SlotId = entity.SlotId,
                        DayOfWeekNum = entity.DayOfWeekNum,
                        DayOfWeekStr = ((DayOfWeek)entity.DayOfWeekNum).ToString(),
                        Time = entity.Time,
                        MaxPerSlot = entity.MaxPerSlot,
                        CreateAt = entity.CreateAt,
                        CreateName = entity.User.UserName
                    };
            }
        }

        public SlotDetail GetMaxTimeSlot()
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity =
                    ctx
                        .TimeSlots
                        .Where(e => e.SlotId > 0).OrderByDescending(x => x.DayOfWeekNum).ThenByDescending(x => x.Time).FirstOrDefault();
                return
                    new SlotDetail
                    {
                        SlotId = entity.SlotId,
                        DayOfWeekNum = entity.DayOfWeekNum,
                        DayOfWeekStr = ((DayOfWeek)entity.DayOfWeekNum).ToString(),
                        Time = entity.Time,
                        MaxPerSlot = entity.MaxPerSlot,
                        CreateAt = entity.CreateAt,
                        CreateName = entity.User.UserName
                    };
            }
        }

        public bool CreateTimeSlot(SlotCreate model)
        {
            var entity =
                new TimeSlot()
                {
                    DayOfWeekNum = model.DayOfWeekNum,
                    Time = model.Time,
                    MaxPerSlot = model.MaxPerSlot,
                    CreateBy = _userId,
                    CreateAt = DateTimeOffset.Now
                };
            using (var ctx = new ApplicationDbContext())
            {
                ctx.TimeSlots.Add(entity);

                bool success = true;
                try { ctx.SaveChanges(); }
                catch { success = false; }

                return success;
            }
        }

        public bool UpdateTimeSlot(SlotUpdate model)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity =
                    ctx
                        .TimeSlots
                        .Single(e => e.SlotId == model.SlotId);
                entity.DayOfWeekNum = model.DayOfWeekNum;
                entity.Time = model.Time;
                entity.MaxPerSlot = model.MaxPerSlot;

                bool success = true;
                try { ctx.SaveChanges(); }
                catch { success = false; }

                return success;
            }
        }

        public bool DeleteTimeSlot(int id)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity =
                    ctx
                        .TimeSlots
                        .Single(e => e.SlotId == id);

                ctx.TimeSlots.Remove(entity);

                bool success = true;
                try { ctx.SaveChanges(); }
                catch { success = false; }

                return success;
            }
        }
    }
}
