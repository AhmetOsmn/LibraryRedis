using Core.Utilities.Security.JWT;
using Entities.Concrete;
using Entities.DTOs;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IUserService
    {
        Task<User> Register(RegisterDTO u);
        Task<User> Login(string username, string password);
        Task<bool> UserExist(string username);
        Task<AccessToken> CreateAccessToken(User user);
        Task<User> GetUserByUsername(string userName);
    }
}
