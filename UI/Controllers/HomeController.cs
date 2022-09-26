using Entities.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Threading.Tasks;
using UI.Abstract;
using UI.Models;

namespace UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IRequestService _requestService;

        public HomeController(ILogger<HomeController> logger, IRequestService requestService)
        {
            _logger = logger;
            _requestService = requestService;
        }
        [HttpGet]
        public IActionResult SignIn()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SignIn(UserLoginDTO dto)
        {
            if (dto == null)
            {
                return RedirectToAction("Register");
            }

            var login = await _requestService.CreateLoginRequest(dto);

            if (login != null)
            {
                var userInfo = await _requestService.GetLoggedInUserInformation(login);
                HttpContext.Session.SetString("user", JsonConvert.SerializeObject(userInfo));
                HttpContext.Session.SetString("token", JsonConvert.SerializeObject(login));

                if (userInfo.RoleID.ToString() == "2")
                {
                    return RedirectToAction("Index", "Admin");
                }
                if (userInfo.RoleID.ToString() == "1")
                {
                    return RedirectToAction("Index", "User");
                }
            }
            //Giriş olan kullanıcı çalışan admin yönlendirilecek
            return View();
        }
        [HttpGet]
        public IActionResult Register()
        {
            if (HttpContext.Session.GetString("user") != null)
                return RedirectToAction("Privacy", "Home");
            //Kayıt ol kısmına yönlendirileck
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterDTO dto)
        {
            var user = await _requestService.AddRegister(dto);
            if (user == null)
            {
                ViewBag.ErrorMessage = "Kullanıcı adı kullanılmaktadır.";
                return View();
            }
            return RedirectToAction("SignIn");
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
