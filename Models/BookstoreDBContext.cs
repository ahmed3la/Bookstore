using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bookstore.Models
{
    public class BookstoreDBContext:DbContext
    {
        public BookstoreDBContext(DbContextOptions <BookstoreDBContext> options):base (options)
        {

        } 
        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }

        public static readonly ILoggerFactory MyLoggerFactory = LoggerFactory.Create(builder => {
            builder.AddDebug();
        });
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
 
            optionsBuilder.UseLoggerFactory(MyLoggerFactory);
        }
    }
}
