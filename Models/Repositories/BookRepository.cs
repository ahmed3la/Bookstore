using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bookstore.Models.Repositories
{
    public class BookRepository : IBookstoreRepository<Book>
    {
        private readonly BookstoreDBContext dbContext;
        public BookRepository(BookstoreDBContext _dBContext)
        {
            this.dbContext = _dBContext;
        }
        public void Add(Book book)
        {
            dbContext.Books.Add(book);
            dbContext.SaveChanges();
        }

        public void Delete(int id)
        {
            var book = Find(id);
            dbContext.Books.Remove(book);
            dbContext.SaveChanges();
        }

        public Book Find(int id)
        {
            var book = dbContext.Books.Include(a => a.Author).FirstOrDefault(a => a.Id == id);
            return book;
        }

        public IList<Book> List()
        {
            return dbContext.Books.Include(a => a.Author).ToList();
        }

        public IList<Book> Search(Func<Book, bool> predicate)
        {
            var list = dbContext.Books.Include(a => a.Author).Where(predicate);
            return list.ToList();
        }

        public void Update(int id,Book newBook)
        {
            dbContext.Books.Update(newBook);
            dbContext.SaveChanges();


        }
    }
}
