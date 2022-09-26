using Business.Abstract;
using DAL.Abstract;
using Entities.Concrete;
using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class BookManager : IBookService
    {
        private readonly IBookDAL _bookDal;
        private readonly IFileService _fileService;
        private readonly IUserService _userService;
        public BookManager(IBookDAL bookDal, IFileService fileService, IUserService userService)
        {
            _bookDal = bookDal;
            _fileService = fileService;
            _userService = userService;
        }

        public async Task AddBook(BookDTO dto)
        {
            var model = new Book
            {
                IsApproved = true,
                IsReserved = false,
                Name = dto.Name,
                PageCount = dto.PageCount,
                UserID = dto.UserId.Value,
                PublishYear = dto.PublishYear,
                Summary = dto.Summary,
                ApprovedUser = dto.UserId.ToString(),
                PhotoPath = dto.PhotoPath,
                Author = dto.Author
            };
            try
            {
                await _bookDal.AddAsync(model);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        public async Task<IEnumerable<Book>> GetAllActiveBooksAsync()
        {
            return await _bookDal.GetAllAsync(b => b.IsApproved && !b.IsReserved);
        }

        public async Task<Book> GetBookByBookId(int bookId)
        {
            var book = await _bookDal.GetAsync(x => x.BookID == bookId);
            return book != null ? book : null;
        }

        public async Task<List<Book>> GetBooksByUserID(int ID)
        {
            var books = await _bookDal.GetAllAsync(x => x.UserID == ID && x.IsReserved);
            return books.ToList();
        }

        public void Update(Book book)
        {
            _bookDal.Update(book);
        }

        public async Task<bool> UpdateBookDto(UpdateBookDTO dto, string userName)
        {
            var book = await GetBookByBookId(dto.BookID);
            if (book == null)
                return false;

            var user = await _userService.GetUserByUsername(userName);
            if (user == null)
                return false;

            book.IsReserved = !book.IsReserved;
            book.UserID = user.UserID;
            Update(book);
            return true;
        }
    }
}
