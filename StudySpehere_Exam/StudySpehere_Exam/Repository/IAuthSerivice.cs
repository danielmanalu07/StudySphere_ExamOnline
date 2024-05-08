using StudySpehere_Exam.Models;

namespace StudySpehere_Exam.Repository
{
    public interface IAuthSerivice
    {
        Admin AuthenticateAdmin(string username, string password);
        Student AuthenticateStudent(string username, string password);
        Student RegisterStudent(string namalengkap, string username, string password);
    }
}
