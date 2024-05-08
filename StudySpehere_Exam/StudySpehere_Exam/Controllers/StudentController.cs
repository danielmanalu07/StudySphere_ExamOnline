using Microsoft.AspNetCore.Mvc;
using StudySpehere_Exam.Data;
using StudySpehere_Exam.Repository;

namespace StudySpehere_Exam.Controllers
{
    public class StudentController : Controller
    {
        private readonly IHttpContextAccessor httpContextAccessor;
		private readonly AppDbContext db;
        private readonly IAuthSerivice authSerivice;

        public StudentController(IHttpContextAccessor _httpContextAccessor, AppDbContext _db, IAuthSerivice _authSerivice)
        {
            httpContextAccessor = _httpContextAccessor;
            db = _db;
            authSerivice = _authSerivice;
        }
        public IActionResult Dashboard()
        {
            var session = httpContextAccessor.HttpContext.Session.GetString("Student");
            if (string.IsNullOrEmpty(session))
            {
                TempData["error"] = "Must Be Login";
                return RedirectToAction("Index", "Home");
            }
			var student = db.students.FirstOrDefault(s => s.Username == session);
			return View(student);
        }

        public IActionResult Logout()
        {
            httpContextAccessor.HttpContext.Session.Remove("Student");
            httpContextAccessor.HttpContext.Session.Clear();
            TempData["success"] = "Logged out Successfully";
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(string namalengkap, string username, string password)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var student = authSerivice.RegisterStudent(namalengkap, username, password);
            if (student == null)
            {
                TempData["error"] = "Invalid Register";
                return RedirectToAction("Register", "Student");
            }
            db.students.Add(student);
            db.SaveChanges();
            TempData["success"] = "Register student Successfully";
            return RedirectToAction("Index", "Home");
        }
    }
}
