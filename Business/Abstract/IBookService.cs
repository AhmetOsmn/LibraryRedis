using Entities.Concrete;
using Entities.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IBookService
    {
        Task AddBook(BookDTO dto);
        Task<IEnumerable<Book>> GetAllActiveBooksAsync();
        Task<List<Book>> GetBooksByUserID(int ID);
        Task<Book> GetBookByBookId(int bookId);
        Task<bool> UpdateBookDto(UpdateBookDTO dto, string userName);
        void Update(Book book);

    }
}
