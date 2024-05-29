using Microsoft.AspNetCore.Mvc;
using ProjectMVC.Data;
using ProjectMVC.Models;
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
        [HttpGet]
        public IActionResult Create()
        {
            return View("Form");
        }
        [HttpPost]
        public IActionResult Create(AuthorFormVM authorFormVM)
        {
            if (!ModelState.IsValid)
            {
                return View("Form", authorFormVM);
            }
            var auther = new Author
            {
                Id = authorFormVM.Id,
                Name = authorFormVM.Name,
            };
            _context.Authors.Add(auther);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var authorId = _context.Authors.Find(id);
            if (authorId == null) { return NotFound(); }
            var authorForm = new AuthorFormVM
            {
                Id = authorId.Id,
                Name = authorId.Name,
            };
            return View("Form", authorForm);
        }
        [HttpPost]
        public IActionResult Edit(AuthorFormVM authorFormVM)
        {
            if (!ModelState.IsValid)
            {
                return View("Form",authorFormVM) ;
            }
            var NewAuthor = new Author { Id = authorFormVM.Id, Name = authorFormVM.Name };
            _context.Authors.Update(NewAuthor);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Details(int id)
        {
            var authorData = _context.Authors.Find(id);
            if (authorData == null) { NotFound(); }
            var Authorvm = new AuthorVM
            {
                Id = authorData.Id,
                Name = authorData.Name,
                CreatedAt = authorData.CreatedAt,
                UpdatedAt = authorData.UpdatedAt,
            };
            return View(Authorvm);
        }
        public IActionResult Delete(int id)
        {
            if(!ModelState.IsValid)
            {
                return NotFound();
            }
            var author = _context.Authors.Find(id);
            if (author == null) { NotFound(); }
            _context.Authors.Remove(author);
            _context.SaveChanges();
            return Ok();
        }
    }
}
