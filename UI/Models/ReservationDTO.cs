using System;

namespace UI.Models
{
    public class ReservationDTO
    {
        public int UserID { get; set; }
        public int BookID { get; set; }
        public DateTime StartDate { get; set; } = DateTime.Now;
        public DateTime EndDate { get; set; } = DateTime.Now.AddDays(10);
    }
}
