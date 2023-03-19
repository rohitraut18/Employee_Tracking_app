using EmployeeTrackingWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace EmployeeTrackingWebApp.Controllers
{
    public class AccountController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            return RedirectToAction("Login");
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(SignIn sign)
        {
            AppUser user = db.AppUsers.Where(u => u.EmailId == sign.Username && u.Password == sign.Password).FirstOrDefault();

            if (user == null)
            {
                return Json(new { status = "error", message = "Enter valid email and password." }, JsonRequestBehavior.AllowGet);
            }

            FormsAuthentication.SetAuthCookie(sign.Username, false);

            var jsdata = new
            {
                user.AppUserId,
                user.FullName,
                user.MobileNo,
                user.EmailId,
                user.AppRoleId,
                user.AppRole.AppRoleName
            };

            return Json(new { status = "ok", message = "You have logged in successfully.", data=jsdata}, JsonRequestBehavior.AllowGet);
        }
         

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut(); 
            return RedirectToAction("", "Account");
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