using Entities.Abstract;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Concrete
{
    public class Book : IEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BookID { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public string Summary { get; set; }
        public int PageCount { get; set; }
        public DateTime PublishYear { get; set; }
        public bool IsReserved { get; set; }
        public string PhotoPath { get; set; }
        [ForeignKey("User")]
        public int UserID { get; set; }
        public bool IsApproved { get; set; }
        public string ApprovedUser { get; set; }

        public virtual User User { get; set; }


    }
}
