using Microsoft.AspNetCore.Mvc;
using ProjectMVC.Data;
using ProjectMVC.ViewModels.Author;

namespace ProjectMVC.Controllers
{
    public class AuthorsController : Controller
    {
        private readonly ApplicationDbContext _context;
        public AuthorsController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var authers = _context.Authors.ToList();
            var auther = new List<AuthorVM>();//view model بعرف ليست من ال
            foreach (var author in authers)
            {// authorVM ل  author وبعمل عليها لوب عشان احولها من 
                var authorVM = new AuthorVM
                {
                    Id = author.Id,
                    Name = author.Name,
                    CreatedAt = author.CreatedAt,
                    UpdatedAt = author.UpdatedAt,
                };
                auther.Add(authorVM);// وبكل لفة بضيفها على الليست الي عرفتها 
            }
            return View(auther);
        }
    }
}
