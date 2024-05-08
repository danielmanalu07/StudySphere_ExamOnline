using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace StudySpehere_Exam.Models
{
    public class CategoryQuiz
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Name is required")]
        [DisplayName("Name")]
        public string? Name { get; set; }
        [Required(ErrorMessage = "Description is required")]
        [DisplayName("Description")]
        public string? Description { get; set; }
    }
}
