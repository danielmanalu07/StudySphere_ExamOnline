using Microsoft.EntityFrameworkCore;
using StudySpehere_Exam.Models;

namespace StudySpehere_Exam.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Admin> admins { get; set; }
        public DbSet<Role> roles { get; set; }
        public DbSet<Student> students { get; set; }
        public DbSet<CategoryQuiz> categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Admin>().HasData(
                new Admin
                {
                    Id = 1,
                    Username = "admin",
                    Password = "admin12345",
                    RoleId = 1,
                }
                );
            modelBuilder.Entity<Role>().HasData(
                new Role
                {
                    Id = 1,
                    Name = "admin",
                },
                new Role
                {
                    Id = 2,
                    Name = "student",
                }
                );
            modelBuilder.Entity<Student>().HasData(
                new Student
                {
                    Id = 1,
                    NamaLengkap = "Daniel Manalu",
                    Username = "Daman07",
                    Password = "daman12345",
                    RoleId = 2,
                }
                );
        }
    }
}
