using Microsoft.AspNetCore.Mvc;
using StudySpehere_Exam.Data;
using StudySpehere_Exam.Models;

namespace StudySpehere_Exam.Controllers
{
    public class CategoryQuizController : Controller
    {
        private readonly AppDbContext db;

        public CategoryQuizController(AppDbContext _db)
        {
            db = _db;
        }
        public IActionResult Index()
        {
            var cat = db.categories.ToList();
            return View(cat);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(string name, string description)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var cat = new CategoryQuiz
            {
                Name = name,
                Description = description,
            };

            db.categories.Add(cat);
            db.SaveChanges();
            TempData["success"] = "Created Category Successfully";
            return View(cat);
        }
    }
}
