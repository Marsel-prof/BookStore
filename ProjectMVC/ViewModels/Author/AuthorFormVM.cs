using System.ComponentModel.DataAnnotations;

namespace ProjectMVC.ViewModels.Author
{
    public class AuthorFormVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "The Name must be between 3 and 30 character")] // بهاي الطريقة بغير نص الايرور الي بدي يظهر لليوزر
        [MaxLength(30)]
        [MinLength(3)]
        public string Name { get; set; }
    }
}
