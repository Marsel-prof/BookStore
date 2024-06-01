using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace ProjectMVC.ViewModels.Book
{
    public class BookFormVM
    {
        public int Id { get; set; }

        public string Title { get; set; }

        [Display(Name = "Author")]
        public int AuthorId { get; set; }// عشان اعمل سيليكت لاوبشين واحد

        public List<SelectListItem> Authors { get; set; }
        public string Publisher { get; set; } = null!;

        [Display(Name = "Publish Date")]
        public DateTime PublishDate { get; set; } = DateTime.Now;

        [Display(Name = "Please choose image")]
        public IFormFile? ImageUrl { get; set; }

        public string Description { get; set; } = null!;
        [Display(Name = "Categories")]
        public List<int> SelectCategories { get; set; } = new List<int>(); // عشان اقدر اعمل سيليكت لاكثر من اوبشين
        public List<SelectListItem> Categories { get; set; }
    }
}
