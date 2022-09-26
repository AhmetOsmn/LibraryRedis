using Core.DataAccess.EntityFramework;
using DAL.Abstract;
using Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Concrete.EntityFramework
{
    public class EFUserDAL : EfEntityRepositoryBase<User, DatabaseContext>, IUserDAL
    {
        public async Task<string> GetUserRoleByUsername(string userName)
        {
            using (DatabaseContext db = new DatabaseContext())
            {
                string role = await (from u in db.User
                                     join r in db.Role
                                     on u.RoleID equals r.RoleID
                                     where u.UserName == userName
                                     select r.Name).FirstOrDefaultAsync();
                return role;
            }
        }
    }
}
