

using Business.Abstract;
using Entities.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginDTO dto)
        {
            var result = await _userService.Login(dto.Username, dto.Password);
            if (result == null)
                return BadRequest();
            var tokenResult = await _userService.CreateAccessToken(result);
            return Ok(tokenResult);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDTO dto)
        {
            var result = await _userService.Register(dto);
            if (result == null)
                return BadRequest();
            //var tokenResult = await _userService.CreateAccessToken(result);
            return Ok(dto.UserName + "kayıt oldu");
        }
    }
}
