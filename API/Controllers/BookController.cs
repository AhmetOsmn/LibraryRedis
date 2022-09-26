using Business.Abstract;
using DAL.Abstract;
using Entities.Concrete;
using Entities.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookService;
        private readonly IUserService _userService;
        private readonly IRedisCacheService _redisCacheService;

        public BookController(IBookService bookService, IUserService userService, IRedisCacheService redisCacheService)
        {
            _bookService = bookService;
            _userService = userService;
            _redisCacheService = redisCacheService;
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddBook(BookDTO dto)
        {
            await _bookService.AddBook(dto);
            return Ok();
        }
        [HttpPost("update")]
        public async Task<IActionResult> UpdateBook(UpdateBookDTO dto)
        {
            var username = HttpContext.User.Identity.Name;
            bool result = await _bookService.UpdateBookDto(dto, username);
            if (result)
            {
                return Ok();
            }
            return BadRequest();
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            // todo: redis manager'a baglanti var mı metodu eklenecek

            var result = await _bookService.GetAllActiveBooksAsync();
            return Ok(result);
        }

        [HttpGet("GetReservedBook")]
        public async Task<IActionResult> GetReservedBook()
        {
            var username = HttpContext.User.Identity.Name;
            var user = await _userService.GetUserByUsername(username);

            var books = await _bookService.GetBooksByUserID(user.UserID);
           
            return Ok(books);
        }

    }

}
