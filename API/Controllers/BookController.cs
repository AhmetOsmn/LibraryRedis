using Business.Abstract;
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

        public BookController(IBookService bookService, IUserService userService)
        {
            _bookService = bookService;
            _userService = userService;
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

        [HttpGet("getall")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _bookService.GetAllActiveBooksAsync();
            return Ok(result);
        }

        [HttpGet("GetReservedBooks")]
        public async Task<IActionResult> GetReservedBook(int userId)
        {
            //var username = HttpContext.User.Identity.Name;
            //var user = await _userService.GetUserByUsername(username);

            var books = await _bookService.GetBooksByUserID(userId);
           
            return Ok(books);
        }

    }

}
