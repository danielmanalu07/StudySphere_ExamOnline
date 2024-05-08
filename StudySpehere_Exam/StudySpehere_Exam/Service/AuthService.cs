using StudySpehere_Exam.Data;
using StudySpehere_Exam.Models;
using StudySpehere_Exam.Repository;

namespace StudySpehere_Exam.Service
{
    public class AuthService : IAuthSerivice
    {
        private readonly AppDbContext _context;

        public AuthService(AppDbContext context)
        {
            _context = context;
        }

        public Admin AuthenticateAdmin(string username, string password)
        {
            return _context.admins.FirstOrDefault(a => a.Username == username && a.Password == password);
        }

        public Student AuthenticateStudent(string username, string password)
        {
            return _context.students.FirstOrDefault(s => s.Username == username && s.Password == password);
        }

        public Student RegisterStudent(string namalengkap, string username, string password)
        {
            var students = new Student
            {
                NamaLengkap = namalengkap,
                Username = username,
                Password = password,
                RoleId = 2,
            };
            return students;
        }
    }
}
