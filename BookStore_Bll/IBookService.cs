using BookStore_Dal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore_Bll
{
    public interface IBookService
    {
        Task<IEnumerable<Book>> GetBooksAsync();
        Task<Book?> AddBookAsync(Book book);
        Task SaveChangesAsync();
        void DeleteBook(Book book);
        Task<Book?> GetBookAsync(int id);
    }
}
