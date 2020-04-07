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
                return ctx.SaveChanges() == 1;
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

                return ctx.SaveChanges() == 1;
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

                return ctx.SaveChanges() == 1;
            }
        }
    }
}
