using Entities.Concrete;
using Entities.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Threading.Tasks;
using UI.Abstract;
using UI.DTOs;
using UI.Models;

namespace UI.Controllers
{
    public class UserController : Controller
    {
        private readonly IRequestService _requestService;

        public UserController(IRequestService requestService)
        {
            _requestService = requestService;
        }

        // todo: alt kısımlarda token session üzerinden alınıyor. Redis'ten mi alınmalı?

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

            // yeni rezervasyon oluşturulacak. EndDate olarak default 10 gün verilecek.
            int currentUserID = JsonConvert.DeserializeObject<User>(HttpContext.Session.GetString("user")).UserID;

            var result = await _requestService.AddReservation(new ReservationDTO { BookID = dto.BookID, UserID = currentUserID },tokenDto);

            if(result == "kayit basarili")
            {
                await _requestService.UpdateBook(tokenDto, dto);
                return Ok();
            }
            return BadRequest();
        }

        [HttpGet]
        public async Task<IActionResult> ReservedBooks()
        {
            var tokenDto = JsonConvert.DeserializeObject<TokenDTO>(HttpContext.Session.GetString("token"));

            int currentUserID = JsonConvert.DeserializeObject<User>(HttpContext.Session.GetString("user")).UserID;

            var reservations = await _requestService.GetReservations(tokenDto, currentUserID);
            return View(reservations);
        }     
        
        [HttpPost]
        public async Task<IActionResult> ReservedBooks(UpdateBookDTO dto)
        {
            var tokenDto = JsonConvert.DeserializeObject<TokenDTO>(HttpContext.Session.GetString("token"));

            int currentUserID = JsonConvert.DeserializeObject<User>(HttpContext.Session.GetString("user")).UserID;

            var result = await _requestService.DeleteReservation(new Models.ReservationDeleteDTO { BookId = dto.BookID, UserId = currentUserID }, tokenDto);

            if (result == "silme basarili")
            {
                await _requestService.UpdateBook(tokenDto, dto);
                return Ok();
            }
            return BadRequest();
        }


        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("SignIn", "Home");
        }

        public IActionResult Information()
        {
            return View();
        }
    }
}
