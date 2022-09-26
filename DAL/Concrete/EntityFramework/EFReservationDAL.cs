using Core.DataAccess.EntityFramework;
using DAL.Abstract;
using Entities.Concrete;
using Entities.DTOs;
using System.Collections.Generic;
using System.Linq;

namespace DAL.Concrete.EntityFramework
{
    public class EFReservationDAL : EfEntityRepositoryBase<Reservation, DatabaseContext>, IReservationDAL
    {

        public IEnumerable<ReservationDTOGet> GetAllWithNamesByUserId(int userId)
        {
            IEnumerable<ReservationDTOGet> reservations;
            using (DatabaseContext db = new DatabaseContext())
            {
                reservations = (from res in db.Reservation
                                join u in db.User on res.UserID equals u.UserID
                                join b in db.Book on res.BookID equals b.BookID
                                where res.IsActive == true && u.UserID == userId
                                select new ReservationDTOGet
                                {
                                    BookName = b.Name,
                                    UserName = u.UserName,
                                    BookSummary = b.Summary,
                                    BookID = b.BookID,
                                    PhotoPath = b.PhotoPath,
                                    StartDate = res.StartDate.ToShortDateString(),
                                    EndDate = res.EndDate.ToShortDateString()
                                }
                                ).ToList();
            }
            return reservations;
        }

        public ReservationDTOGet GetReservationDetailForCache(int userId, int bookId)
        {
            ReservationDTOGet reservation;
            using (DatabaseContext db = new DatabaseContext())
            {
                reservation = (from res in db.Reservation
                                join u in db.User on res.UserID equals u.UserID
                                join b in db.Book on res.BookID equals b.BookID
                                where res.IsActive == true && u.UserID == userId && b.BookID == bookId
                                select new ReservationDTOGet
                                {
                                    BookName = b.Name,
                                    UserName = u.UserName,
                                    BookSummary = b.Summary,
                                    BookID = b.BookID,
                                    PhotoPath = b.PhotoPath,
                                    StartDate = res.StartDate.ToShortDateString(),
                                    EndDate = res.EndDate.ToShortDateString()
                                }).FirstOrDefault();
            }
            return reservation;
        }
    }
}
