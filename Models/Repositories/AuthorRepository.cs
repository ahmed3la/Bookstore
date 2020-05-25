using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bookstore.Models.Repositories
{
    public class AuthorRepository : IBookstoreRepository<Author>
    {
        private readonly BookstoreDBContext dbContext;

        public AuthorRepository(BookstoreDBContext _dBContext)
        {
            this.dbContext = _dBContext;
        }
        public void Add(Author author)
        {

            dbContext.Authors.Add(author);
            dbContext.SaveChanges();
        }

        public void Delete(int id)
        {
            var author = Find(id);
            dbContext.Authors.Remove(author);
            dbContext.SaveChanges();
        }

        public Author Find(int id)
        {
            var author = dbContext.Authors.SingleOrDefault(a => a.Id == id);
            return author;
        }

        public IList<Author> List()
        {
            return dbContext.Authors.ToList();
        }

        public IList<Author> Search(Func<Author, bool> predicate)
        {
            return dbContext.Authors.Where(predicate).ToList();
        }

        public void Update(int id,Author newAuthor)
        {
            dbContext.Authors.Update(newAuthor);
            dbContext.SaveChanges();
        }
 
    }
}
