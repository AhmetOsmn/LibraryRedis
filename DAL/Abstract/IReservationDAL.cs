using Core.DataAccess;
using Entities.Concrete;
using Entities.DTOs;
using System.Collections.Generic;

namespace DAL.Abstract
{
    public interface IReservationDAL : IEntityRepository<Reservation>
    {
        ReservationDTOGet GetReservationDetailForCache(int userId, int bookId);
        IEnumerable<ReservationDTOGet> GetAllWithNamesByUserId(int userId);
    }
}
