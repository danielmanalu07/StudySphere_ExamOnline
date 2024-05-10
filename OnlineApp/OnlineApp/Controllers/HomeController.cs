using OnlineApp.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace OnlineApp.Controllers
{
	public class HomeController : Controller
	{
		DBEXAMEntities db = new DBEXAMEntities();
		score sc = new score();
        int scoreExam = 0;


		[HttpGet]
		public ActionResult Tlogin()
		{
			return View();
		}

		public ActionResult Logout()
		{
			Session.Abandon();
			Session.RemoveAll();
			return RedirectToAction("Index");
		}
		[HttpPost]
		public ActionResult Tlogin(Admin ad)
		{
			Admin adm = db.Admins.Where(x => x.ad_name == ad.ad_name && x.ad_password == ad.ad_password).SingleOrDefault();
			if (adm != null)
			{
				Session["ad_id"] = adm.ad_id;
				return RedirectToAction("Dashboard");
			}
			else
			{
				ViewBag.msg = "Invalid Login";
			}
			return View();
		}
		[HttpGet]
		public ActionResult sregister()
		{
			return View();
		}
		[HttpPost]
		public ActionResult sregister(Student svm, HttpPostedFileBase img)
		{
			Student s = new Student();
			try
			{
				s.st_name = svm.st_name;
				s.st_password = svm.st_password;
				s.st_image = uploadimage(img);
				db.Students.Add(s);
				db.SaveChanges();
				return RedirectToAction("Slogin");

			}
			catch (Exception)
			{
				ViewBag.msg = "Couldn't Register";
			}
			return View(svm);
		}

		private string uploadimage(HttpPostedFileBase file)
		{
			string path = "-1";

			if (file != null && file.ContentLength > 0)
			{
				string extension = Path.GetExtension(file.FileName).ToLower();
				string[] allowedExtensions = { ".jpg", ".jpeg", ".png" };

				if (allowedExtensions.Contains(extension))
				{
					try
					{
						Random r = new Random();
						int random = r.Next();

						string fileName = random + Path.GetFileName(file.FileName);
						path = Path.Combine(Server.MapPath("~/Content/Image/"), fileName);

						file.SaveAs(path);

						path = "~/Content/Image/" + fileName;
					}
					catch (Exception ex)
					{
						path = "-1";
					}
				}
				else
				{
					ViewBag.msg = "Only jpg, jpeg, or png formats are acceptable.";
				}
			}
			else
			{
				ViewBag.msg = "Please select a file.";
			}

			return path;
		}

		public ActionResult Slogin()
		{
			return View();
		}
		[HttpPost]
		public ActionResult Slogin(Student s)
		{
			Student st = db.Students.Where(x => x.st_name == s.st_name && x.st_password == s.st_password).SingleOrDefault();


			if (st == null)
			{
				ViewBag.msg = "Invalid Login";
			}
			else
			{
				Session["std_id"] = st.st_id;
				return RedirectToAction("ExamDashboard");
			}
			return View();
		}

		public ActionResult ExamDashboard()
		{
			if (Session["std_id"] == null)
			{
				return RedirectToAction("Slogin");
			}
			List<Category> cat = db.Categories.ToList();
			return View(cat);
		}
		[HttpPost]
		public ActionResult ExamDashboard(string room)
		{
			if (Session["std_id"] == null)
			{
				return RedirectToAction("Slogin");
			}
			List<Category> list = db.Categories.ToList();
			foreach (var item in list)
			{
				if (item.cat_encrypted_string == room)
				{
					List<Question> li = db.Questions.Where(x => x.q_fk_catid == item.cat_id).ToList();
					Queue<Question> queue = new Queue<Question>();
                    foreach (Question a in li)
                    {
						queue.Enqueue(a);
                    }

					TempData["examid"] = item.cat_fk_ad_id;
					TempData["questions"] = queue;

					TempData["score"] = 0;

					TempData.Keep();
					return RedirectToAction("StartQuiz");
				}
				else
				{
					ViewBag.error = "No Room Found";
				}
			}
			return View();
		}

		public ActionResult LogoutSt()
		{
			if (Session["std_id"] == null)
			{
				return RedirectToAction("Slogin");
			}

			Session.Remove("std_id");

			return RedirectToAction("Index");
		}

		public ActionResult StartQuiz()
		{
			if (Session["std_id"] == null)
			{
				return RedirectToAction("Slogin");
			}

			Question q = null;
            if (TempData["questions"] != null)
            {
				Queue<Question> qlist = (Queue<Question>)TempData["questions"];
                if (qlist.Count > 0)
                {
					q = qlist.Peek();
					qlist.Dequeue();

					TempData["questions"] = qlist;
					TempData.Keep();
                }
                else
                {
					return RedirectToAction("EndExam");
                }
            }
            else
            {
				return RedirectToAction("ExamDashboard");
			}
			TempData.Keep();
			return View(q);
        }
		[HttpPost]
		public ActionResult StartQuiz(Question q)
		{
			// Determine the selected answer
			string selectedAnswer = null;
			if (q.opA != null)
			{
				selectedAnswer = "A";
			}
			else if (q.opB != null)
			{
				selectedAnswer = "B";
			}
			else if (q.opC != null)
			{
				selectedAnswer = "C";
			}
			else if (q.opD != null)
			{
				selectedAnswer = "D";
			}

			string correctAnswer = q.Cop;

			if (selectedAnswer != null && selectedAnswer.Equals(correctAnswer))
			{
				// Retrieve the current score from TempData or initialize to 0
				int currentScore = TempData["score"] != null ? Convert.ToInt32(TempData["score"]) : 0;

				currentScore++;

				TempData["score"] = currentScore;
			}

			return RedirectToAction("StartQuiz");
		}


		public ActionResult ViewAllQuestion(int? id, int page = 1)
		{
			if (Session["ad_id"] == null)
			{
				return RedirectToAction("Tlogin");
			}
			if (id == null)
			{
				return RedirectToAction("Dashboard");
			}

			return View(db.Questions.Where(x => x.q_fk_catid == id).ToList());
		}

		public ActionResult EditQuestion(int id)
		{
			if (Session["ad_id"] == null)
			{
				return RedirectToAction("Tlogin");
			}

			var question = db.Questions.Find(id);
			if (question == null)
			{
				return HttpNotFound();
			}

			return View(question);
		}

		[HttpPost]
		public ActionResult EditQuestion(Question question)
		{
			if (Session["ad_id"] == null)
			{
				return RedirectToAction("Tlogin");
			}

			if (ModelState.IsValid)
			{
				var existingQuestion = db.Questions.Find(question.qu_id);
				if (existingQuestion != null)
				{
					existingQuestion.qu_text = question.qu_text;
					existingQuestion.opA = question.opA;
					existingQuestion.opB = question.opB;
					existingQuestion.opC = question.opC;
					existingQuestion.opD = question.opD;
					existingQuestion.Cop = question.Cop;

					db.Entry(existingQuestion).State = EntityState.Modified;
					db.SaveChanges();

					return RedirectToAction("ViewAllQuestion");
				}
				else
				{
					return HttpNotFound();
				}
			}

			return View(question);
		}
		[HttpPost]
		public ActionResult DeleteConfirmed(int id)
		{
			if (Session["ad_id"] == null)
			{
				return RedirectToAction("Tlogin");
			}

			var question = db.Questions.Find(id);
			if (question == null)
			{
				return HttpNotFound();
			}

			db.Questions.Remove(question);
			db.SaveChanges();
			return RedirectToAction("ViewAllQuestion"); // Redirect kembali ke halaman daftar pertanyaan
		}




		public ActionResult EndExam()
		{
			if (Session["std_id"] == null)
			{
				return RedirectToAction("Slogin");
			}

			int score = TempData["score"] != null ? Convert.ToInt32(TempData["score"]) : 0;

			return View(score);
		}


		public ActionResult Dashboard()
		{
			if (Session["ad_id"] == null)
			{
				return RedirectToAction("TLogin");
			}
			return View();
		}
		[HttpGet]
		public ActionResult Add_Category()
		{
			// Session["ad_id"] = 1;

			int ad_id = Convert.ToInt32(Session["ad_id"].ToString());

			List<Category> catLi = db.Categories.Where(c => c.cat_fk_ad_id == ad_id).OrderBy(x => x.cat_id).ToList();
			ViewData["list"] = catLi;
			return View();
		}

		[HttpPost]
		public ActionResult Add_Category(Category cat)
		{
			List<Category> catLi = db.Categories.OrderByDescending(x => x.cat_id).ToList();
			ViewData["list"] = catLi;
			Category c = new Category();
			Random r = new Random();
			c.cat_name = cat.cat_name;

			c.cat_fk_ad_id = Convert.ToInt32(Session["ad_id"].ToString());
			c.cat_encrypted_string = crypt.Encrypt(cat.cat_name.Trim() + r.Next().ToString(), true);

			db.Categories.Add(c);
			db.SaveChanges();

			return RedirectToAction("Add_Category");
		}

		[HttpGet]
		public ActionResult AddQuestions()
		{
			int sid = Convert.ToInt32(Session["ad_id"]);
			List<Category> li = db.Categories.Where(x => x.cat_fk_ad_id == sid).ToList();
			ViewBag.list = new SelectList(li, "cat_id", "cat_name");
			return View();
		}

		[HttpPost]
		public ActionResult AddQuestions(Question q)
		{
			int sid = Convert.ToInt32(Session["ad_id"]);
			List<Category> li = db.Categories.Where(x => x.cat_fk_ad_id == sid).ToList();
			ViewBag.list = new SelectList(li, "cat_id", "cat_name");

			Question qu = new Question();
			qu.qu_text = q.qu_text;
			qu.opA = q.opA;
			qu.opB = q.opB;
			qu.opC = q.opC;
			qu.opD = q.opD;
			qu.Cop = q.Cop;

			qu.q_fk_catid = q.q_fk_catid;


			db.Questions.Add(qu);
			db.SaveChanges();
			TempData["ms"] = "Question Successfully Added";
			TempData.Keep();
			return RedirectToAction("AddQuestions");
		}


		public ActionResult Index()
		{
			if (Session["ad_id"] != null)
			{
				return RedirectToAction("Dashboard");
			}
			return View();
		}

		public ActionResult About()
		{
			ViewBag.Message = "Your application description page.";

			return View();
		}

		public ActionResult Contact()
		{
			ViewBag.Message = "Your contact page.";

			return View();
		}

		public ActionResult recors()
		{
			ViewBag.Message = "Your contact page.";

			return View();
		}
	}
}