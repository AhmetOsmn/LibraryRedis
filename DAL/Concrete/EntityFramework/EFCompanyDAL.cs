using Core.DataAccess.EntityFramework;
using DAL.Abstract;
using Entities.Concrete;

namespace DAL.Concrete.EntityFramework
{
    public class EFCompanyDAL : EfEntityRepositoryBase<Company, DatabaseContext>, ICompanyDAL
    {
    }
}
