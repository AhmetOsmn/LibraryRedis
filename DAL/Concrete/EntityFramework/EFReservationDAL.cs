using Core.DataAccess.EntityFramework;
using DAL.Abstract;
using Entities.Concrete;
using Entities.DTOs;
using System.Linq;
using System;
using System.Collections.Generic;

namespace DAL.Concrete.EntityFramework
{
    public class EFReservationDAL : EfEntityRepositoryBase<Reservation, DatabaseContext>, IReservationDAL
    {
        public IEnumerable<ReservationDTOGet> GetAllWithNames()
        {
            IEnumerable<ReservationDTOGet> reservations;
            using (DatabaseContext db = new DatabaseContext())
            {
                reservations = (from res in db.Reservation
                                join u in db.User on res.UserID equals u.UserID
                                join b in db.Book on res.BookID equals b.BookID
                                where res.IsActive == true
                                select new ReservationDTOGet
                                {
                                    BookName = b.Name,
                                    UserName = u.UserName,
                                    BookSummary = b.Summary,
                                    PhotoPath = b.PhotoPath,
                                    StartDate = res.StartDate.ToShortDateString(),
                                    EndDate = res.EndDate.ToShortDateString()
                                }
                                ).ToList();
            }
            return reservations;
        }

        public IEnumerable<ReservationDTOGet> GetAllWithNamesByUserID(int userId)
        {
            IEnumerable<ReservationDTOGet> reservations;
            using (DatabaseContext db = new DatabaseContext())
            {
                reservations = (from res in db.Reservation
                                join u in db.User on res.UserID equals u.UserID
                                join b in db.Book on res.BookID equals b.BookID
                                where res.IsActive == true && res.UserID == userId
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
    }
}
