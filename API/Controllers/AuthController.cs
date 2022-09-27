using Business.Abstract;
using Core.Utilities.Security.JWT;
using DAL.Abstract;
using Entities.Concrete;
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
        private readonly IRedisCacheService redisCacheService;

        public AuthController(IUserService userService, IRedisCacheService redisCacheService)
        {
            _userService = userService;
            this.redisCacheService = redisCacheService;
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginDTO dto)
        {
            // todo: login islemleri redis ile yapilacak mi?
            // todo: cok fazla if blogu oldu gibi, daha iyi yazilabilir mi?


            AccessToken tokenFromDb;

            if (redisCacheService.IsConnected())
            {
                var user = redisCacheService.Get<User>("user:" + dto.Username);

                if(user == null)
                {
                    var userFromDb = await _userService.Login(dto.Username, dto.Password);

                    if(userFromDb == null)
                    {
                        return BadRequest();
                    }
                    redisCacheService.Add("user:" + userFromDb.UserName,userFromDb);
                    user = userFromDb;
                }

                var tokenFromRedis = redisCacheService.Get<AccessToken>("token:" + dto.Username);

                if (tokenFromRedis != null)
                {
                    return Ok(tokenFromRedis);
                }

                tokenFromDb = await _userService.CreateAccessToken(user);
                redisCacheService.Add("token:" + user.UserName, tokenFromDb, tokenFromDb.Expiration);
                return Ok(tokenFromDb);
            }
            else
            {
                var result = await _userService.Login(dto.Username, dto.Password);
                if (result == null)
                    return BadRequest();

                var tokenResult = await _userService.CreateAccessToken(result);
                return Ok(tokenResult);
            }

        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDTO dto)
        {
            var user = await _userService.Register(dto);
            if (user == null)
                return BadRequest();

            if(redisCacheService.IsConnected())
                redisCacheService.Add("user:"+user.UserName, user);
            
            return Ok(dto.UserName + "kayıt oldu");
        }
    }
}
