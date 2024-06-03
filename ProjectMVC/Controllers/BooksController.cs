using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NuGet.ContentModel;
using NuGet.Protocol;
using ProjectMVC.Data;
using ProjectMVC.Data.Migrations;
using ProjectMVC.Models;
using ProjectMVC.ViewModels;
using ProjectMVC.ViewModels.Book;
using System.Collections.Immutable;

namespace ProjectMVC.Controllers
{
    public class BooksController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment webHostEnvironment;
        public BooksController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            this.webHostEnvironment = webHostEnvironment;
            _context = context;
        }
        public IActionResult Index()
        {
            var books = _context.Books.Include(book => book.Author) // الانكلود بستخدمها عشان اجيب داتا من تيبل ثاني
                .Include(book => book.Categories)
                .ThenInclude(book => book.category)// الذن انكلود بتستخدمها عشان اجيب داتا جوا ليست جوا تيبل
                // يعني هون الكاتيجوري عندي ليست اول اشي بجيب الليست بعدها بالذن انكلود بجيب كل كاتيجوري جوا الليست
                .ToList()
                .Select(book => new BookVM // طريقة مختصرة للمابنج بدل اللوب 
                 {
                    Id = book.Id,
                    Title = book.Title,
                    ImageUrl = book.ImageUrl,
                    Authors = book.Author.Name,
                    PublishDate = book.PublishDate,
                    Publisher = book.Publisher,
                    Categories = book.Categories.Select(c => c.category.Name).ToList(),
                 }).ToList();
           
            return View(books);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var authors = _context.Authors.OrderBy(author => author.Name)
                .Select(author => new SelectListItem
                {
                    Value = author.Id.ToString(),
                    Text = author.Name
                })
                .ToList();
           
            var categories = _context.Categories.OrderBy(category => category.Name)
                .Select(category => new SelectListItem
                {
                    Value = category.Id.ToString(),
                    Text = category.Name
                })
                .ToList();
          
            var ViewModle = new BookFormVM
            {
                Authors = authors,
                Categories = categories,
            };
            return View(ViewModle);
        }
        [HttpPost]
        public IActionResult Create(BookFormVM bookFormVM)
        {
            if (ModelState.IsValid)
            {
                return View(bookFormVM);
            }
            string? fileName = null;
            if (bookFormVM.ImageUrl != null)
            {
                 fileName = Path.GetFileName(bookFormVM.ImageUrl.FileName);
                var pathFile = Path.Combine($"{webHostEnvironment.WebRootPath}/img/books",fileName);
                using (var stream = new FileStream(pathFile, FileMode.Create))
                {
                    bookFormVM.ImageUrl.CopyTo(stream);
                }
            }
            var Books = new Book
            {
                ImageUrl = fileName,
                Id = bookFormVM.Id,
                Title = bookFormVM.Title,
                AuthorId = bookFormVM.AuthorId,
                Description = bookFormVM.Description,
                PublishDate = bookFormVM.PublishDate,
                Publisher = bookFormVM.Publisher,
                // لهيك بعملها هيك بدل ما اعمل لوب BookCategory هون انا لازم الداتا الي عندي تتخزن ك 
                Categories = bookFormVM.SelectCategories.Select(id => new BookCategory
                {
                    CategoryId = id,
                }).ToList(),
            };
            _context.Books.Add(Books);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Delete(int id)
        {
            var book = _context.Books.Find(id);
            if (book == null)
            {
                return NotFound();
            }

            var path = Path.Combine(webHostEnvironment.WebRootPath, "img/books", book.ImageUrl);
            if (System.IO.File.Exists(path))
            {
                try
                {
                    System.IO.File.Delete(path);
                }
                catch (IOException ex)
                {
                    // Log the exception and provide feedback
                    Console.WriteLine($"IOException: {ex.Message}");
                    return StatusCode(500, "Error deleting file. Please try again later.");
                }
            }

            _context.Books.Remove(book);
            _context.SaveChanges();
            return Ok();
        }

    }

}
