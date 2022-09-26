using Entities.Abstract;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Concrete
{
    public class User : IEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserID { get; set; }
        public string NameSurname { get; set; }
        public string UserName { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Adress { get; set; }
        [ForeignKey("Role")]
        public int RoleID { get; set; }
        [ForeignKey("Company")]
        public int? CompanyID { get; set; }

        public virtual Role Role { get; set; }
        public virtual Company Company { get; set; }
    }
}
