using Business.Abstract;
using Core.Utilities.Security;
using Core.Utilities.Security.JWT;
using DAL.Abstract;
using Entities.Concrete;
using Entities.DTOs;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class UserManager : IUserService
    {
        private readonly IUserDAL _dalUser;
        private readonly ITokenHelper _tokenHelper;

        public UserManager(IUserDAL dalUser, ITokenHelper tokenHelper)
        {
            _dalUser = dalUser;
            _tokenHelper = tokenHelper;
        }

        public async Task<User> Login(string username, string password)
        {
            var user = await _dalUser.GetAsync(a => a.UserName == username);

            if (user == null)
            {
                return null;
            }
            else
            {
                if (!VerifyPassword(password, user.PasswordHash, user.PasswordSalt))
                {
                    return null;
                }
            }
            return user;
        }

        private bool VerifyPassword(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            return HashingHelper.VerifyPasswordHash(password, passwordHash, passwordSalt);
        }

        public async Task<User> Register(RegisterDTO dto)
        {
            if (await UserExist(dto.UserName))
            {
                return null;
            }
            byte[] passwordHash, passwordSalt;
            HashingHelper.CreatePasswordHash(dto.Password, out passwordHash, out passwordSalt);
            var user = new User
            {
                Email = dto.Email,
                NameSurname = dto.NameSurname,
                Adress = dto.Adress,
                PhoneNumber = dto.PhoneNumber,
                RoleID = 1,
                UserName = dto.UserName,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
            };

            await _dalUser.AddAsync(user);
            return user;
        }

        public async Task<bool> UserExist(string username)
        {
            //todo: redis'ten mi bakacağız, db'den mi bakacağız?

            var user = await _dalUser.GetAsync(x => x.UserName == username);

            return user != null ? true : false;
        }

        public async Task<AccessToken> CreateAccessToken(User user)
        {
            //Validation
            string role = await _dalUser.GetUserRoleByUsername(user.UserName);
            var accessToken = _tokenHelper.CreateToken(user, role);
            return accessToken;
        }

        public async Task<User> GetUserByUsername(string userName)
        {
            var user = await _dalUser.GetAsync(x => x.UserName == userName);
            return user != null ? user : null;
        }
    }
}
