using Core.DataAccess.EntityFramework;
using DAL.Abstract;
using Entities.Concrete;

namespace DAL.Concrete.EntityFramework
{
    public class EFRoleDAL : EfEntityRepositoryBase<Role, DatabaseContext>, IRoleDAL
    {
    }
}
