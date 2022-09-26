using API.Models;
using Entities.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IReservationService
    {
        Task<bool> AddReservationAsync(ReservationDTO dto);
        bool DeleteReservation(int reservationId);
        IEnumerable<ReservationDTOGet> GetReservations();
        IEnumerable<ReservationDTOGet> GetReservationsByUserID(int userID);
    }
}
