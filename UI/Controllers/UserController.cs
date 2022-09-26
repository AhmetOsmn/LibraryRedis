using Entities.Concrete;
using Entities.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Threading.Tasks;
using UI.Abstract;
using UI.DTOs;

namespace UI.Controllers
{
    public class UserController : Controller
    {
        private readonly IRequestService _requestService;

        public UserController(IRequestService requestService)
        {
            _requestService = requestService;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var tokenDto = JsonConvert.DeserializeObject<TokenDTO>(HttpContext.Session.GetString("token"));

            var books = await _requestService.GetAllBooks(tokenDto);
            return View(books);
        }
        [HttpPost]
        public async Task<IActionResult> Index(UpdateBookDTO dto)
        {
            var tokenDto = JsonConvert.DeserializeObject<TokenDTO>(HttpContext.Session.GetString("token"));
            await _requestService.UpdateBook(tokenDto, dto);
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> ReservedBooks()
        {
            var tokenDto = JsonConvert.DeserializeObject<TokenDTO>(HttpContext.Session.GetString("token"));

            // todo: burada user sınıfını kullanmak ne kadar dogru?
            string currentUserName = JsonConvert.DeserializeObject<User>(HttpContext.Session.GetString("user")).UserName;
            var reservations = await _requestService.GetReservations(tokenDto, currentUserName);
            return View(reservations);
        }
        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            // todo: redisten veri silinecek.
            return RedirectToAction("SignIn", "Home");
        }

        public IActionResult Information()
        {
            return View();
        }
    }
}
