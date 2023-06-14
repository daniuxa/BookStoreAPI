using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore_Dal
{
    public class BookStoreDbContext : DbContext
    {
        public DbSet<Book> Books { get; set; } = null!;

        public BookStoreDbContext(DbContextOptions<BookStoreDbContext> options) : base(options)
        {

        }
    }
}
