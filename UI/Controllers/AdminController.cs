using Business.Abstract;
using DAL.Abstract;
using Entities.Concrete;
using Entities.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using UI.Abstract;
using UI.DTOs;

namespace UI.Controllers
{
    public class AdminController : Controller
    {
        private readonly IRequestService _requestService;
        private readonly IFileService _fileService;
        private readonly IRedisCacheService _cacheService;
        public AdminController(IRequestService requestService, IFileService fileService, IRedisCacheService cacheService)
        {
            _requestService = requestService;
            _fileService = fileService;
            _cacheService = cacheService;
        }

        User currentUser;

        [HttpGet]
        public IActionResult Index()
        {
            currentUser = JsonConvert.DeserializeObject<User>(HttpContext.Session.GetString("user"));

            return View();
        }

        [HttpGet]
        public IActionResult AddCompany()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddCompany(CompanyDTO dto)
        {
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

            await _requestService.AddCompany(dto, tokenDto);
            return RedirectToAction("AddCompany");
        }
        [HttpGet]
        public async Task<IActionResult> AddBook()
        {
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

            var compaines = await _requestService.GetAllCompanies(tokenDto);
            var items = new List<SelectListItem>();

            compaines.ForEach(c =>
            {
                items.Add(new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                });
            });
            ViewBag.Companies = items;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddBook(BookDTO dto)
        {
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

            dto.UserId = currentUser.UserID;
            dto.IsApproved = true;
            dto.IsReserved = false;
            dto.PhotoPath = await _fileService.SaveFileToLocalDirectory(dto.Photo);
            dto.Photo = null;
            await _requestService.AddBook(dto, tokenDto);
            return RedirectToAction("AddBook");
        }

        [HttpGet]
        public IActionResult LogOut()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("SignIn", "Home");
        }

    }
}
