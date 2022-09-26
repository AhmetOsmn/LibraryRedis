using Microsoft.AspNetCore.Http;
using System;

namespace Entities.DTOs
{
    public class BookDTO
    {
        public string Name { get; set; }
        public string Author { get; set; }
        public string Summary { get; set; }
        public int PageCount { get; set; }
        public DateTime PublishYear { get; set; }
        public bool? IsReserved { get; set; }
        public IFormFile Photo { get; set; }
        public string PhotoPath { get; set; }
        public bool? IsApproved { get; set; }
        public string ApprovedUser { get; set; }
        public int CompanyId { get; set; }
        public int? UserId { get; set; }

    }
}
