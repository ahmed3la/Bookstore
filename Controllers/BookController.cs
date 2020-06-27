using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using Bookstore.Models;
using Bookstore.Models.Repositories;
using Bookstore.ViewModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Bookstore.Controllers
{
    public class BookController : Controller
    {
        private readonly IBookstoreRepository<Book> bookRepository;
        private readonly IBookstoreRepository<Author> authorRepository;
        private readonly IWebHostEnvironment hosting;

        public BookController(IBookstoreRepository<Book> bookRepository,
            IBookstoreRepository<Author> authorRepository,
            IWebHostEnvironment Hosting 
            )
        {
            this.bookRepository = bookRepository;
            this.authorRepository = authorRepository;
            hosting = Hosting; 
        }



        // GET: Book
        public ActionResult Index()
        {
            var books = bookRepository.List();
            return View(books);
        }

        // GET: Book/Details/5
        public ActionResult Details(int id)
        {
            var book = bookRepository.Find(id);
            return View(book);
        }

        // GET: Book/Create
        public ActionResult Create()
        { 

            var model = new BookAuthorViewModel() { 
                Authors= fillSelectList()
            };


            return View(model);
        }

        // POST: Book/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BookAuthorViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string fileName = UploadFile(model.File) ?? string.Empty;
                 
                    if (model.AuthorId == -1)
                    {
                        ViewBag.Message = "Please select an author";
                         
                        return View(GetAllAuthors());
                    }

                    var author = authorRepository.Find(model.AuthorId);

                    // TODO: Add insert logic here
                    Book book = new Book()
                    {
                        Id = model.Id,
                        Title = model.Title,
                        Description = model.Description,
                        Author = author,
                        ImageURL= fileName
                    };
                    bookRepository.Add(book);

                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    return View();
                }

            }
            else {
                ModelState.AddModelError("", "You have to fill all required fields");
            }
            return View(GetAllAuthors()); ;
        }
        // GET: Book/Edit/5
        public ActionResult Edit(int id)
        {
            var book = bookRepository.Find(id);
            var authorID = book.Author == null ? 0 : book.Author.Id;

            var viewModel = new BookAuthorViewModel() {
                Id = book.Id,
                Description = book.Description,
                Title = book.Title,
                AuthorId = authorID,
                Authors = fillSelectList(),
                ImageURL=book.ImageURL
            };
             
            return View(viewModel);
        }

        // POST: Book/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(BookAuthorViewModel viewModel)
        {
            string fileName = viewModel.ImageURL;
            // TODO: Add update logic here
            try
            {
                if (viewModel.File != null)
                {
                    fileName = UploadFile(viewModel.File) ?? string.Empty;

                    string oldFileName = viewModel.ImageURL;
                    viewModel.ImageURL = "";
                    //Delete old file 
                    DeleteOldFile(oldFileName, viewModel.File.FileName);
                }

                var author = authorRepository.Find(viewModel.AuthorId);
                Book book = new Book
                {
                    Id = viewModel.Id,
                    Title = viewModel.Title,
                    Description = viewModel.Description,
                    Author = author,
                    ImageURL = fileName
                };

                bookRepository.Update(viewModel.Id, book);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        // GET: Book/Delete/5
        public ActionResult Delete(int id)
        {
            var book = bookRepository.Find(id);
            return View(book);
        }

        // POST: Book/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ConfirmDelete(int id)
        {
            try
            {
                // TODO: Add delete logic here
                bookRepository.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Search(string term)
        {
            List<Book> list = new List<Book>();
            if (!string.IsNullOrEmpty(term))
                list = bookRepository.Search(a => a.Title.ToLower().Contains(term.ToLower())
                                                 || a.Description.ToLower().Contains(term.ToLower())
                                                 || a.Author.FullName.ToLower().Contains(term.ToLower())).ToList();
            else
                list = bookRepository.List().ToList();
            return View("Index", list);
        }
        List<Author> fillSelectList()
        {

            var authors = authorRepository.List().ToList();
            authors.Insert(0, new Author() { Id = -1, FullName = "--- Please select an author ---" });
            return authors;
        }

        BookAuthorViewModel GetAllAuthors() 
        {
            var vModel = new BookAuthorViewModel()
            {
                Authors = fillSelectList()
            };

            return vModel;
        }


        #region <<<<<<<<<Helper>>>>>>>>>
        string UploadFile(IFormFile file)
        {
            if (file != null)
            {
                var uploads = Path.Combine(hosting.WebRootPath, "uploads"); 
                var fullPath = Path.Combine(uploads, file.FileName);

                System.GC.Collect();
                System.GC.WaitForPendingFinalizers();
                file.CopyTo(new FileStream(fullPath, mode: FileMode.Create));
                return file.FileName;
            }
            return null;
        }

        bool DeleteOldFile(string oldFileName, string newFileName)
        { 
            if (oldFileName != newFileName)
            {
                var uploads = Path.Combine(hosting.WebRootPath, "uploads"); 
                if (!string.IsNullOrWhiteSpace(oldFileName))
                {
                    string fullOldPath = Path.Combine(uploads, oldFileName);
                    System.GC.Collect();
                    System.GC.WaitForPendingFinalizers();
                    System.IO.File.Delete(fullOldPath);
                } 

            }

            return false;
        }

        #endregion



    }
}