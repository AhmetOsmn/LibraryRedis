using Business.Abstract;
using DAL.Abstract;

namespace Business.Concrete
{
    public class RoleManager : IRoleService
    {
        private readonly IRoleDAL _dalRole;

        public RoleManager(IRoleDAL dalRole)
        {
            _dalRole = dalRole;
        }
    }
}
