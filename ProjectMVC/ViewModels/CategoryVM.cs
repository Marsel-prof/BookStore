using System.ComponentModel.DataAnnotations;

namespace ProjectMVC.ViewModels
{
    public class CategoryVM
    {
        public int Id { get; set; }
        // هون بعمل اي فاليديشن بدي اياه 
        [Required(ErrorMessage = "The Name must be between 3 and 30 character")] // بهاي الطريقة بغير نص الايرور الي بدي يظهر لليوزر
        [MaxLength(30)]
        [MinLength(3)]
        public string Name { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public DateTime UpdatedOn { get; set; } = DateTime.Now;
    }

}

