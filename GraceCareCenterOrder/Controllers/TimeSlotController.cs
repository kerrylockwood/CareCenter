using CareModels.TimeSlots;
using CareServices;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GraceCareCenterOrder.Controllers
{
    [Authorize]
    public class TimeSlotController : Controller
    {
        // GET: TimeSlot
        public ActionResult Index()
        {
            var service = CreateTimeSlotService();
            var model = service.GetTimeSlots();
            return View(model.OrderBy(o => o.DayOfWeekNum).ThenBy(o => o.Time
                ));
        }

        // Get: TimeSlot/Create
        public ActionResult Create()
        {
            ViewBag.DayOfWeekNum = BuildDayOfWeekDropdown(0);

            return View();
        }

        // POST: TimeSlot/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SlotCreate model)
        {
            if (!ModelState.IsValid) 
            {
                ViewBag.DayOfWeekNum = BuildDayOfWeekDropdown(model.DayOfWeekNum);
                return View(model); 
            }

            if (TimeSpan.Compare(model.Time, TimeSpan.Zero) < 0 || TimeSpan.Compare(model.Time, new TimeSpan(23,59,59)) > 0)
            {
                ModelState.AddModelError("", "Please enter Time using a 24-hour clock in the format HH:MM");
                ViewBag.DayOfWeekNum = BuildDayOfWeekDropdown(model.DayOfWeekNum);
                return View(model);
            }

            var service = CreateTimeSlotService();

            if (service.CreateTimeSlot(model))
            {
                TempData["SaveResult"] = $"'{model.DayOfWeekNum.ToString()} at {model.Time}' was created";
                return RedirectToAction("Index");
            };

            ModelState.AddModelError("", $"'{model.DayOfWeekNum.ToString()} at {model.Time}' could not be created.");
            ViewBag.DayOfWeekNum = BuildDayOfWeekDropdown(model.DayOfWeekNum);
            return View(model);
        }

        // GET: TimeSlot/Details
        public ActionResult Details(int id)
        {
            var svc = CreateTimeSlotService();
            var model = svc.GetTimeSlotById(id);

            return View(model);
        }

        // GET: TimeSlot/Update
        public ActionResult Edit(int id)
        {
            var service = CreateTimeSlotService();
            var detail = service.GetTimeSlotById(id);
            var model =
                new SlotUpdate
                {
                    SlotId = detail.SlotId,
                    DayOfWeekNum = detail.DayOfWeekNum,
                    Time = detail.Time,
                    MaxPerSlot = detail.MaxPerSlot
                };

            ViewBag.DayOfWeekNum = BuildDayOfWeekDropdown(detail.DayOfWeekNum);

            return View(model);
        }

        // GET: TimeSlot/Update
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, SlotUpdate model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.DayOfWeekNum = BuildDayOfWeekDropdown(model.DayOfWeekNum);
                return View(model);
            }

            if (TimeSpan.Compare(model.Time, TimeSpan.Zero) < 0 || TimeSpan.Compare(model.Time, new TimeSpan(23, 59, 59)) > 0)
            {
                ModelState.AddModelError("", "Please enter Time using a 24-hour clock in the format HH:MM");
                ViewBag.DayOfWeekNum = BuildDayOfWeekDropdown(model.DayOfWeekNum);
                return View(model);
            }

            if (model.SlotId != id)
            {
                ModelState.AddModelError("", "Id Mismatch");
                ViewBag.DayOfWeekNum = BuildDayOfWeekDropdown(model.DayOfWeekNum);
                return View(model);
            }

            var service = CreateTimeSlotService();

            if (service.UpdateTimeSlot(model))
            {
                TempData["SaveResult"] = $"'{model.DayOfWeekNum.ToString()} at {model.Time}' was updated.";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", $"'{model.DayOfWeekNum.ToString()} at {model.Time}' could not be updated.");
            ViewBag.DayOfWeekNum = BuildDayOfWeekDropdown(model.DayOfWeekNum);
            return View(model);
        }

        // GET: TimeSlot/Delete
        public ActionResult Delete(int id)
        {
            var svc = CreateTimeSlotService();
            var model = svc.GetTimeSlotById(id);

            return View(model);
        }

        // GET: TimeSlot/Delete
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeletePost(int id)
        {
            var service = CreateTimeSlotService();

            service.DeleteTimeSlot(id);

            TempData["SaveResult"] = $"Time Slot was deleted";

            return RedirectToAction("Index");
        }

        // Create TimeSlot Service
        private TimeSlotService CreateTimeSlotService()
        {
            var userId = User.Identity.GetUserId();
            var service = new TimeSlotService(userId);
            return service;
        }

        // Build Day Of Week Dropdown
        private IOrderedEnumerable<SelectListItem> BuildDayOfWeekDropdown(int selectedValue)
        {
            List<SlotDayOfWeek> slotDayOfWeekList = new List<SlotDayOfWeek>();
            for (int i = 0; i < 7; i++)
            {
                SlotDayOfWeek item = new SlotDayOfWeek
                {
                    DayOfWeekNum = i,
                    DayOfWeekStr = ((DayOfWeek)i).ToString()
                };
                slotDayOfWeekList.Add(item);
            }

            var SlotDOWList = new SelectList(slotDayOfWeekList, "DayOfWeekNum", "DayOfWeekStr", selectedValue);
            var sortedSlotDOWList = SlotDOWList.OrderBy(o => o.Value);

            return sortedSlotDOWList;
        }
    }
}