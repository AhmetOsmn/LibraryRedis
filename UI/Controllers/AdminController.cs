using Business.Abstract;
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
        public AdminController(IRequestService requestService, IFileService fileService)
        {
            _requestService = requestService;
            _fileService = fileService;
        }
        [HttpGet]
        public IActionResult Index()
        {
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
            var tokenDto = JsonConvert.DeserializeObject<TokenDTO>(HttpContext.Session.GetString("token"));
            await _requestService.AddCompany(dto, tokenDto);
            return RedirectToAction("AddCompany");
        }
        [HttpGet]
        public async Task<IActionResult> AddBook()
        {
            var tokenDto = JsonConvert.DeserializeObject<TokenDTO>(HttpContext.Session.GetString("token"));
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
            var sessUser = JsonConvert.DeserializeObject<Entities.Concrete.User>(HttpContext.Session.GetString("user"));
            var token = JsonConvert.DeserializeObject<TokenDTO>(HttpContext.Session.GetString("token"));
            dto.UserId = sessUser.UserID;
            dto.IsApproved = true;
            dto.IsReserved = false;
            dto.PhotoPath = await _fileService.SaveFileToLocalDirectory(dto.Photo);
            dto.Photo = null;
            await _requestService.AddBook(dto, token);
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
