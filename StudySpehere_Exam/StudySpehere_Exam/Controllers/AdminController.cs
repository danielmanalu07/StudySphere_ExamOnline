using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudySpehere_Exam.Data;

namespace StudySpehere_Exam.Controllers
{
    public class AdminController : Controller
    {
        private readonly IHttpContextAccessor httpContextAccessor;
		private readonly AppDbContext db;

		public AdminController(IHttpContextAccessor _httpContextAccessor, AppDbContext _db)
        {
            httpContextAccessor = _httpContextAccessor;
            db = _db;
        }
        public IActionResult Dashboard()
        {
            var session = httpContextAccessor.HttpContext.Session.GetString("Admin");
            if (string.IsNullOrEmpty(session))
            {
                TempData["error"] = "Must Be Login";
                return RedirectToAction("Index", "Home");
            }
            var admin = db.admins.FirstOrDefault(a => a.Username == session);

            return View(admin);
        }

        public IActionResult Logout()
        {
            httpContextAccessor.HttpContext.Session.Remove("Admin");
            httpContextAccessor.HttpContext.Session.Clear();
            TempData["success"] = "Logged out Successfully";
            return RedirectToAction("Index", "Home");
        }
    }
}
