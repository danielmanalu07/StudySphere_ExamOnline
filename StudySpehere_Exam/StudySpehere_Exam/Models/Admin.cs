using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace StudySpehere_Exam.Models
{
    public class Admin
    {

        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Username is required")]
        [DisplayName("Username")]
        public string? Username { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [DisplayName("Password")]
        public string? Password { get; set; }
        [ForeignKey("RoleId")]
        public int RoleId { get; set; }
        public Role? role { get; set; }
    }
}
