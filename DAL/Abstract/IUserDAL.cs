using Core.DataAccess;
using Entities.Concrete;
using System.Threading.Tasks;

namespace DAL.Abstract
{
    public interface IUserDAL : IEntityRepository<User>
    {
        Task<string> GetUserRoleByUsername(string userName);
    }
}
