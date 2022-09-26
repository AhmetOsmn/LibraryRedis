using System;

namespace API.Models
{
    public class ReservationDTO
    {
        public int UserID { get; set; }
        public int BookID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
