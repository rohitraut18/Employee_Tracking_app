using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using EmployeeTrackingWebApp.Models;

namespace EmployeeTrackingWebApp.Controllers
{
    public class AppUsersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: AppUsers
        public ActionResult Index()
        {
            var appUsers = db.AppUsers.Include(a => a.AppRole); 
            return View(appUsers.ToList());
        }

        // GET: AppUsers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AppUser appUser = db.AppUsers.Find(id);
            if (appUser == null)
            {
                return HttpNotFound();
            }
            return View(appUser);
        }

        // GET: AppUsers/Create
        public ActionResult Create()
        {
            ViewBag.AppRoleId = new SelectList(db.AppRoles, "AppRoleId", "AppRoleName");
            return View();
        }

        // POST: AppUsers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AppUserId,FullName,MobileNo,EmailId,AppRoleId")] AppUser appUser)
        {
            ModelState.Remove("Password");
            if (ModelState.IsValid)
            { 
                appUser.Password = Guid.NewGuid().ToString("N").Substring(0, 8);
                db.AppUsers.Add(appUser);
                db.SaveChanges();

                string msg = string.Format("Congratulation! your employee account has been created. Login id:{0} and Password:{1}", appUser.EmailId, appUser.Password);
                EmailSender.SendEmail(appUser.EmailId, "Your accound details", msg);
                return RedirectToAction("Index");
            }

            ViewBag.AppRoleId = new SelectList(db.AppRoles, "AppRoleId", "AppRoleName", appUser.AppRoleId);
            return View(appUser);
        }

        // GET: AppUsers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AppUser appUser = db.AppUsers.Find(id);
            if (appUser == null)
            {
                return HttpNotFound();
            }
            ViewBag.AppRoleId = new SelectList(db.AppRoles, "AppRoleId", "AppRoleName", appUser.AppRoleId);
            return View(appUser);
        }

        // POST: AppUsers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AppUserId,FullName,MobileNo,EmailId,AppRoleId")] AppUser appUser)
        {
            ModelState.Remove("Password");
            if (ModelState.IsValid)
            {
                db.Entry(appUser).State = EntityState.Modified;
                db.Entry(appUser).Property(x => x.Password).IsModified = false;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AppRoleId = new SelectList(db.AppRoles, "AppRoleId", "AppRoleName", appUser.AppRoleId);
            return View(appUser);
        }

        // GET: AppUsers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AppUser appUser = db.AppUsers.Find(id);
            if (appUser == null)
            {
                return HttpNotFound();
            }
            return View(appUser);
        }

        // POST: AppUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AppUser appUser = db.AppUsers.Find(id);
            db.AppUsers.Remove(appUser);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
