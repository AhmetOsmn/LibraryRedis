using API.Models;
using Entities.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IReservationService
    {
        Task<bool> AddReservationAsync(ReservationDTO dto);
        bool DeleteReservation(int userId, int bookId);
        IEnumerable<ReservationDTOGet> GetReservationsByUserId(int userId);
        ReservationDTOGet GetReservationDetailForCache(int userId, int bookId);
    }
}
