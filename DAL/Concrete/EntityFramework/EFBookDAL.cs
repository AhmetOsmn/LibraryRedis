using Core.DataAccess.EntityFramework;
using DAL.Abstract;
using Entities.Concrete;

namespace DAL.Concrete.EntityFramework
{
    public class EFBookDAL : EfEntityRepositoryBase<Book, DatabaseContext>, IBookDAL
    {

    }
}
