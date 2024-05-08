using Microsoft.AspNetCore.Mvc;
using StudySpehere_Exam.Data;
using StudySpehere_Exam.Models;
using StudySpehere_Exam.Repository;
using System.Diagnostics;

namespace StudySpehere_Exam.Controllers
{
    public class HomeController : Controller
    {
        private readonly IAuthSerivice authSerivice;
        private readonly AppDbContext db;
        private readonly IHttpContextAccessor httpContextAccessor;

        public HomeController(IAuthSerivice _authSerivice, AppDbContext appDbContext, IHttpContextAccessor _httpContextAccessor)
        {
            authSerivice = _authSerivice;
            db = appDbContext;
            httpContextAccessor = _httpContextAccessor;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Index(string username, string password, string RoleName)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var role = db.roles.FirstOrDefault(r => r.Name == RoleName);
            if (role == null)
            {
                return NotFound();
            }
            if (RoleName == "admin")
            {
                var admin = authSerivice.AuthenticateAdmin(username, password);
                if (admin == null)
                {
                    TempData["error"] = "Invalid Login";
                    return RedirectToAction("Index");
                }
                httpContextAccessor.HttpContext.Session.SetString("Admin", admin.Username);
                TempData["success"] = "Login Successfully";
                return RedirectToAction("Dashboard", "Admin");
            }

            if (RoleName == "student")
            {
                var student = authSerivice.AuthenticateStudent(username, password);
                if (student == null)
                {
                    TempData["error"] = "Invalid Login";
                    return RedirectToAction("Index");
                }
                httpContextAccessor.HttpContext.Session.SetString("Student", student.Username);
                TempData["success"] = "Login Successfully";
                return RedirectToAction("Dashboard", "Student");
            }
            TempData["error"] = "Invalid username or password";
            return View();
        }
    }
}
