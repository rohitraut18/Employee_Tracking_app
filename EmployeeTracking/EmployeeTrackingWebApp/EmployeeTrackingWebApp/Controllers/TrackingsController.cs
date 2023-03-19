using EmployeeTrackingWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EmployeeTrackingWebApp.Controllers
{
    public class TrackingsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();


        public ActionResult LastKnownLocation(int? id)
        {
            if (id == null)
                return RedirectToAction("", "AppUsers");

            Tracking tracking = db.Trackings.Where(t => t.AppUserId == id).OrderByDescending(t => t.OnDateTime).FirstOrDefault();

            AppUser user = db.AppUsers.Find(id);
            if (user == null)
                return RedirectToAction("", "AppUsers");

            ViewBag.AppUserToTrack = user;

            return View(tracking);
           
        }
         
        public ActionResult WayPoints(int? id)
        {
            if (id == null)
                return RedirectToAction("", "AppUsers");

            AppUser user = db.AppUsers.Find(id);
            if (user == null)
                return RedirectToAction("", "AppUsers");

            var hrs = new Dictionary<string, string>();
            var mns = new Dictionary<string, string>();

            for (int i = 0; i < 24; i++)
                hrs.Add(string.Format("{0:00}", i), string.Format("{0:00}", i));

            for (int i = 0; i < 60; i++)
                mns.Add(string.Format("{0:00}", i), string.Format("{0:00}", i));


            ViewBag.FromHours = new SelectList(hrs, "Key", "Value");
            ViewBag.FromMinutes = new SelectList(mns, "Key", "Value");

            ViewBag.AppUserToTrack = user;

            return View(new List<Tracking>());
        }

        [HttpPost]
        public ActionResult WayPoints(TrackingDuration duration)
        {
            if (duration == null)
                return RedirectToAction("", "AppUsers");

            AppUser user = db.AppUsers.Find(duration.AppUserId);
            if (user == null)
                return RedirectToAction("", "AppUsers");


            DateTime fdt = duration.FromDate.Value.AddHours(duration.FromHours).AddMinutes(duration.FromMinutes);
            DateTime tdt = duration.ToDate.Value.AddHours(duration.ToHours).AddMinutes(duration.ToMinutes);

            var trackings = db.Trackings.Where(t => t.AppUserId == duration.AppUserId && (t.OnDateTime >= fdt && t.OnDateTime <= tdt)).ToList();


            var hrs = new Dictionary<string, string>();
            var mns = new Dictionary<string, string>();

            for (int i = 0; i < 24; i++)
                hrs.Add(string.Format("{0:00}", i), string.Format("{0:00}", i));

            for (int i = 0; i < 60; i++)
                mns.Add(string.Format("{0:00}", i), string.Format("{0:00}", i));


            ViewBag.FromHours = new SelectList(hrs, "Key", "Value");
            ViewBag.FromMinutes = new SelectList(mns, "Key", "Value");

            ViewBag.AppUserToTrack = user;
             
            return View(trackings);
        }

        public ActionResult Create(Tracking tracking)
        { 
            ModelState.Remove("OnDateTime");
            if (ModelState.IsValid)
            {
                AppUser LoggedUser = db.AppUsers.FirstOrDefault(u => u.EmailId == User.Identity.Name);

                tracking.AppUserId = LoggedUser.AppUserId;
                tracking.OnDateTime = DateTime.Now;
                db.Trackings.Add(tracking);
                db.SaveChanges();
                return Json(new { status = "ok" }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { status = "error", message = "Data validation failed." }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetLastKnownLocation(int? AppUserId)
        {
            if (AppUserId == null)
                return Json(new { status = "error", message = "Data validation failed." }, JsonRequestBehavior.AllowGet);

            Tracking tracking = db.Trackings.Where(t => t.AppUserId == AppUserId).OrderByDescending(t => t.OnDateTime).FirstOrDefault();

            if (tracking == null)
                return Json(new { status = "error", message = "No location found" }, JsonRequestBehavior.AllowGet);

            var jt = new
            {
                tracking.TrackingId,
                tracking.AppUserId,
                tracking.Lat,
                tracking.Long,
                OnDateTime = tracking.OnDateTime.ToString("dd MMM yy hh:mm:ss tt")
            };

            return Json(new { status = "ok", message = "location found", data = jt }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult GetWayPoints(int? AppUserId, DateTime? From, DateTime? To)
        {
            if (AppUserId == null)
                return Json(new { status = "error", message = "Data validation failed." }, JsonRequestBehavior.AllowGet);

            if (From == null)
                From = DateTime.Today;

            if (To == null)
                To = DateTime.Today.AddHours(23).AddMinutes(59);

            var trackings = db.Trackings.Where(t => t.AppUserId == AppUserId && t.OnDateTime >= From && t.OnDateTime <= To).OrderBy(t => t.OnDateTime).ToList();


            var jts = from tracking in trackings
                      select new
                      {
                          tracking.TrackingId,
                          tracking.AppUserId,
                          tracking.Lat,
                          tracking.Long,
                          OnDateTime = tracking.OnDateTime.ToString("dd MMM yy hh:mm:ss tt")
                      };

            return Json(new { status = "ok", message = "location found", data = jts }, JsonRequestBehavior.AllowGet);

        }


    }
}