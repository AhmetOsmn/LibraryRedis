using Entities.Abstract;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Concrete
{
    public class Reservation : IEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ReservationID { get; set; }
        [ForeignKey("Book")]
        public int BookID { get; set; }
        [ForeignKey("User")]
        public int UserID { get; set; }
        public DateTime StartDate { get; set; } = DateTime.Now;
        public DateTime EndDate { get; set; } = DateTime.Now.AddDays(15);
        public virtual Book Book { get; set; }
        public virtual User User { get; set; }
        public bool IsActive { get; set; } = true;

    }
}
