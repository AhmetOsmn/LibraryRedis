using API.Models;
using Business.Abstract;
using DAL.Abstract;
using Entities.Concrete;
using Entities.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class ReservationManager : IReservationService
    {
        private readonly IReservationDAL _dalReservation;

        public ReservationManager(IReservationDAL dalReservation)
        {
            _dalReservation = dalReservation;
        }

        public async Task<bool> AddReservationAsync(ReservationDTO dto)
        {
            var reservation = _dalReservation.Get(x => x.UserID == dto.UserID && x.BookID == dto.BookID);

            if (reservation == null)
            {
                await _dalReservation.AddAsync(new Reservation
                {
                    UserID = dto.UserID,
                    BookID = dto.BookID,
                    StartDate = dto.StartDate,
                    EndDate = dto.EndDate
                });

                return true;
            }
            // todo: enddate ve startdate farklı olursa burası yanlış işlem yapacaktır. Burası düzenlenmeli
            else if (!reservation.IsActive)
            {
                reservation.IsActive = true;
                reservation.EndDate = dto.EndDate;
                reservation.StartDate = dto.StartDate;
                _dalReservation.Update(reservation);
                return true;
            }

            return false;
        }

        public bool DeleteReservation(int userId, int bookId)
        {
            var reservationToDelete = _dalReservation.Get(x => x.IsActive && x.UserID == userId && x.BookID == bookId);

            if (reservationToDelete == null) return false;

            reservationToDelete.IsActive = false;
            _dalReservation.Update(reservationToDelete);

            return true;
        }

        public IEnumerable<ReservationDTOGet> GetReservationsByUserId(int userId)
        {
            return _dalReservation.GetAllWithNamesByUserId(userId);
        }
        public ReservationDTOGet GetReservationDetailForCache(int userId, int bookId)
        {
            return _dalReservation.GetReservationDetailForCache(userId, bookId);
        }
    }
}
