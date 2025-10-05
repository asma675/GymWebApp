using System.Web.Mvc;
using GymWebApp.Models;
using GymWebApp.Repositories; 

namespace GymWebApp.Controllers
{
    public class AdminController : Controller
    {
        private bool IsAdmin => (Session["is_admin"] as bool?) == true;

        [HttpGet]
        public ActionResult Login() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string username, string password)
        {
            if (string.Equals(username, "admin") && string.Equals(password, "password123"))
            {
                Session["is_admin"] = true;
                return RedirectToAction(nameof(Dashboard));
            }

            ViewBag.Error = "That did not match our records.";
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Logout()
        {
            Session["is_admin"] = null;
            return RedirectToAction(nameof(Login));
        }

        [HttpGet]
        public ActionResult Dashboard()
        {
            if (!IsAdmin) return RedirectToAction(nameof(Login));
            return View();
        }

        [HttpGet]
        public ActionResult Members()
        {
            if (!IsAdmin) return RedirectToAction(nameof(Login));
            var all = AppState.Members.GetAll();   
            return View(all);
        }

        [HttpGet]
        public ActionResult Search(string email, string phone)
        {
            if (!IsAdmin) return RedirectToAction(nameof(Login));

            var repo = AppState.Members;          
            Member found = null;

            if (!string.IsNullOrWhiteSpace(email))
                found = repo.FindByEmail(email);
            else if (!string.IsNullOrWhiteSpace(phone))
                found = repo.FindByPhone(phone);

            ViewBag.NotFound = (found == null);
            return View(found);
        }
    }
}
