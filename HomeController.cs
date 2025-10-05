using System;
using System.Linq;
using System.Web.Mvc;
using GymWebApp.AppData;
using GymWebApp.Models;
using GymWebApp.Repositories;

namespace GymWebApp.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Index() => View();

        [HttpGet]
        public ActionResult Fees()
        {
            ViewBag.Fees = new[]
            {
                new { Plan = "Monthly",   Price = 50 },
                new { Plan = "Quarterly", Price = 135 },
                new { Plan = "Yearly",    Price = 500 },
            };
            return View();
        }

        [HttpGet]
        public ActionResult Join()
        {
            ViewBag.MembershipOptions = new[] { "Monthly", "Quarterly", "Yearly" };
            return View(new Member());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Join(Member model)
        {
            ViewBag.MembershipOptions = new[] { "Monthly", "Quarterly", "Yearly" };

            if (!ModelState.IsValid)
                return View(model);

            var digits = (model.Phone ?? string.Empty).Count(ch => char.IsDigit(ch));
            if (digits < 10)
            {
                ModelState.AddModelError(nameof(model.Phone), "Please include area code (10+ digits total).");
                return View(model);
            }

            if (string.Equals(model.MembershipType, "Yearly", StringComparison.OrdinalIgnoreCase))
            {
                model.YearlyDiscountPercent = new Random().Next(1, 101);
            }

            if (!AppState.Members.AddMember(model, out string error))
            {
                if (!string.IsNullOrEmpty(error) && error.IndexOf("email", StringComparison.OrdinalIgnoreCase) >= 0)
                    ModelState.AddModelError(nameof(model.Email), error);
                else if (!string.IsNullOrEmpty(error) && error.IndexOf("phone", StringComparison.OrdinalIgnoreCase) >= 0)
                    ModelState.AddModelError(nameof(model.Phone), error);
                else
                    ModelState.AddModelError("", error ?? "Could not add member.");

                return View(model);
            }

            TempData["SignedUpName"] = model.Name;
            TempData["SignedUpCount"] = AppState.Members.Count;
            TempData["MembershipType"] = model.MembershipType;
            TempData["Discount"] = model.YearlyDiscountPercent;

            return RedirectToAction(nameof(ThankYou));

        }

        [HttpGet]
        public ActionResult ThankYou()
        {
            ViewBag.SignedUpName = TempData["SignedUpName"] ?? "New Member";
            ViewBag.SignedUpCount = TempData["SignedUpCount"] ?? 1;
            ViewBag.MembershipType = TempData["MembershipType"] as string;
            ViewBag.Discount = TempData["Discount"] as int?;
            return View();
        }

        public ActionResult About()
        {
            throw new NotImplementedException();
        }

        public ActionResult Contact()
        {
            throw new NotImplementedException();
        }
    }
}
