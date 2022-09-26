using Core.DataAccess;
using Entities.Concrete;
using Entities.DTOs;
using System.Collections.Generic;

namespace DAL.Abstract
{
    public interface IReservationDAL : IEntityRepository<Reservation>
    {
        IEnumerable<ReservationDTOGet> GetAllWithNames();
        IEnumerable<ReservationDTOGet> GetAllWithNamesByUserID(int userId);
    }
}
