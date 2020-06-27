using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bookstore.Models;
using Bookstore.Models.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Bookstore.Controllers
{
    [Authorize]
    public class AuthorController : Controller
    {
        private readonly IBookstoreRepository<Author> authorRepository; 
        public AuthorController(IBookstoreRepository<Author> authorRepository)
        { 
            this.authorRepository = authorRepository;
        }

        // GET: Author
        [AllowAnonymous]
        public ActionResult Index()
        {
            var author = authorRepository.List();
            return View(author);
        }

        // GET: Author/Details/5
        public ActionResult Details(int id)
        {
            var author = authorRepository.Find(id);
            return View(author);
        }

        // GET: Author/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Author/Create
        [HttpPost] 
        [ValidateAntiForgeryToken]
        public ActionResult Create(Author auther)
        {
            try
            {
                authorRepository.Add(auther);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Author/Edit/5 
        public ActionResult Edit(int id)
        {
            var author = authorRepository.Find(id); 
            return View(author);
        }

        // POST: Author/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
         
        public ActionResult Edit(int id, Author author)
        {
            try
            {
                authorRepository.Update(id, author);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Author/Delete/5
       
        public ActionResult Delete(int id)
        {
            var auther = authorRepository.Find(id); 
            return View(auther);
        }

        // POST: Author/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                authorRepository.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}