using Business.Abstract;
using DAL.Abstract;
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
        private readonly IRedisCacheService _cacheService;

        public UserController(IRequestService requestService, IRedisCacheService cacheService)
        {
            _requestService = requestService;
            _cacheService = cacheService;
        }

        User currentUser;

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            currentUser = JsonConvert.DeserializeObject<User>(HttpContext.Session.GetString("user"));

            TokenDTO tokenDto;

            if (_cacheService.IsConnected())
            {
                tokenDto = _cacheService.Get<TokenDTO>("token:" + currentUser.UserName);

                if(tokenDto == null)
                {
                    tokenDto = JsonConvert.DeserializeObject<TokenDTO>(HttpContext.Session.GetString("token"));
                    _cacheService.Add("token:" + currentUser.UserName, tokenDto, tokenDto.Expiration);
                }
            }
            else
            {
                tokenDto = JsonConvert.DeserializeObject<TokenDTO>(HttpContext.Session.GetString("token"));                
            }

            var books = await _requestService.GetAllBooks(tokenDto);
            return View(books);
        }

        [HttpPost]
        public async Task<IActionResult> Index(UpdateBookDTO dto)
        {
            currentUser = JsonConvert.DeserializeObject<User>(HttpContext.Session.GetString("user"));

            TokenDTO tokenDto;
            if (_cacheService.IsConnected())
            {
                tokenDto = _cacheService.Get<TokenDTO>("token:" + currentUser.UserName);

                if (tokenDto == null)
                {
                    tokenDto = JsonConvert.DeserializeObject<TokenDTO>(HttpContext.Session.GetString("token"));
                    _cacheService.Add("token:" + currentUser.UserName, tokenDto, tokenDto.Expiration);
                }
            }
            else
            {
                tokenDto = JsonConvert.DeserializeObject<TokenDTO>(HttpContext.Session.GetString("token"));
            }


            var result = await _requestService.AddReservation(new ReservationDTO { BookID = dto.BookID, UserID = currentUser.UserID }, tokenDto);

            if (result == "kayit basarili")
            {
                await _requestService.UpdateBook(tokenDto, dto);
                return Ok();
            }
            return BadRequest();
        }

        [HttpGet]
        public async Task<IActionResult> ReservedBooks()
        {
            currentUser = JsonConvert.DeserializeObject<User>(HttpContext.Session.GetString("user"));

            TokenDTO tokenDto;
            if (_cacheService.IsConnected())
            {
                tokenDto = _cacheService.Get<TokenDTO>("token:" + currentUser.UserName);

                if (tokenDto == null)
                {
                    tokenDto = JsonConvert.DeserializeObject<TokenDTO>(HttpContext.Session.GetString("token"));
                    _cacheService.Add("token:" + currentUser.UserName, tokenDto, tokenDto.Expiration);
                }
            }
            else
            {
                tokenDto = JsonConvert.DeserializeObject<TokenDTO>(HttpContext.Session.GetString("token"));
            }


            var reservations = await _requestService.GetReservations(tokenDto, currentUser.UserID);
            return View(reservations);
        }

        [HttpPost]
        public async Task<IActionResult> ReservedBooks(UpdateBookDTO dto)
        {
            currentUser = JsonConvert.DeserializeObject<User>(HttpContext.Session.GetString("user"));

            TokenDTO tokenDto;
            if (_cacheService.IsConnected())
            {
                tokenDto = _cacheService.Get<TokenDTO>("token:" + currentUser.UserName);

                if (tokenDto == null)
                {
                    tokenDto = JsonConvert.DeserializeObject<TokenDTO>(HttpContext.Session.GetString("token"));
                    _cacheService.Add("token:" + currentUser.UserName, tokenDto, tokenDto.Expiration);
                }
            }
            else
            {
                tokenDto = JsonConvert.DeserializeObject<TokenDTO>(HttpContext.Session.GetString("token"));
            }

            var result = await _requestService.DeleteReservation(new Models.ReservationDeleteDTO { BookId = dto.BookID, UserId = currentUser.UserID }, tokenDto);

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
