using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
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
            var authors = _context.Authors.OrderBy(author => author.Name).ToList();
            var authorList = new List<SelectListItem>();
            foreach (var author in authors)
            {
                authorList.Add(new SelectListItem
                {
                    Value = author.Id.ToString(),
                    Text = author.Name,
                });
            }
           
            var categories = _context.Categories.OrderBy(category => category.Name).ToList();
            var categoryList = new List<SelectListItem>();
            foreach (var category in categories)
            {
                categoryList.Add(new SelectListItem
                {
                    Value = category.Id.ToString(),
                    Text = category.Name,
                });
            }
            var ViewModle = new BookFormVM
            {
                Authors = authorList,
                Categories = categoryList,
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
            {// path.getfilename اول اشي بدي اجيب اسم الصورة من خلال شيء جاهز اسمه 
                 fileName = Path.GetFileName(bookFormVM.ImageUrl.FileName);
                // wwwroot هون بدي احدد المكان الي بدي اخزن الصورة فيه من خلال انتر فيس بخليني اقدر اوصل ل 
                //وبحكيلو انو انا رح اخزن الاسم تاع الاصورة 
                var pathFile = Path.Combine($"{webHostEnvironment.WebRootPath}/img/books",fileName);
                // هاد السطر عبارة عن تعامل مع الفايلات زي باقي لغات البرمجة 
                // فهون انا بنشئ مكان جوا الباث الي انا جبتوا عشان اخزن الصورة فيه
                var stream = System.IO.File.Create(pathFile);
                // هون بجيب الصورة من المكان المؤقت الي كانت مخزنة فيه وبنسخها جوا الباث وهيك بكون تخزنت عندي
                bookFormVM.ImageUrl.CopyTo(stream);
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
            if(book == null) { return NotFound(); }
            // لما احذف كتاب لازم احذف الصورة معه عشان هيك بجيب الباث 
            var path = Path.Combine(webHostEnvironment.WebRootPath, "img/books", book.ImageUrl);
            if(System.IO.File.Exists(path))// بفحص اذا الباث موجود بحذفه
            {
                System.IO.File.Delete(path);
            }
            _context.Books.Remove(book);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
   
}
