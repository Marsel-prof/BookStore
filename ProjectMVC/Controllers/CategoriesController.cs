using Microsoft.AspNetCore.Mvc;
using ProjectMVC.Data;
using ProjectMVC.Models;
using ProjectMVC.ViewModels;

namespace ProjectMVC.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ApplicationDbContext _context;
        public CategoriesController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var categories = _context.Categories.ToList();
            return View(categories);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(CategoryVM model)
        {
            if (!ModelState.IsValid)
            {
                return View("Create", model);
            }
            var category = new Category
            {
                Name = model.Name
            };
            try
            {
                _context.Categories.Add(category);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                ModelState.AddModelError("Name", "This Name is already exist");
                return View(model);
            }

        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var category = _context.Categories.Find(id);
            if (category == null)
            {
                return NotFound();
            }
            var categoryVM = new CategoryVM
            {
                Id = id,
                Name = category.Name
            };
            return View("Create", categoryVM);

        }
        [HttpPost]
        public IActionResult Edit(CategoryVM categoryVM)
        {
            if (!ModelState.IsValid)
            {
                return View("Create", categoryVM);
            }

            var category = _context.Categories.Find(categoryVM.Id);
            if (category == null)
            {
                return NotFound();
            }
            category.Name = categoryVM.Name;
            category.UpdatedOn = categoryVM.UpdatedOn;
            _context.Categories.Update(category);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Details(int id)
        {
            var model = _context.Categories.Find(id);
            if (model == null) { return NotFound(); }
            var catergoryVM = new CategoryVM { Id = id, Name = model.Name, CreatedOn = model.CreatedOn, UpdatedOn = model.UpdatedOn };
            return View(catergoryVM);
        }
        public IActionResult Delete(int id)
        {
            var model = _context.Categories.Find(id);
            if (model == null) { return NotFound(); }
            _context.Categories.Remove(model);
            _context.SaveChanges();
            return Ok();
        }
       

    }
}
