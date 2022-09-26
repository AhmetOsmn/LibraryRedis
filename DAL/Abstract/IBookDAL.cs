using Core.DataAccess;
using Entities.Concrete;

namespace DAL.Abstract
{
    public interface IBookDAL : IEntityRepository<Book>
    {
    }
}
