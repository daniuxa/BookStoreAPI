using BookStore_Dal;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore_Bll
{
    public class BookService : IBookService
    {
        private readonly BookStoreDbContext _bookStoreDbContext;

        public BookService(BookStoreDbContext bookStoreDbContext)
        {
            this._bookStoreDbContext = bookStoreDbContext ?? throw new ArgumentNullException(nameof(bookStoreDbContext));
        }

        public async Task<Book?> AddBookAsync(Book book)
        {
            return (await _bookStoreDbContext.Books.AddAsync(book)).Entity;
        }

        public void DeleteBook(Book book)
        {
            _bookStoreDbContext.Books.Remove(book);
        }

        public async Task<Book?> GetBookAsync(int id)
        {
            return await _bookStoreDbContext.Books.Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Book>> GetBooksAsync()
        {
            return await _bookStoreDbContext.Books.ToListAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _bookStoreDbContext.SaveChangesAsync();
        }
    }
}
