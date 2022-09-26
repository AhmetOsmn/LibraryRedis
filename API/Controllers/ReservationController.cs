using API.Models;
using Business.Abstract;
using DAL.Abstract;
using Entities.Concrete;
using Entities.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationController : ControllerBase
    {
        private readonly IReservationService reservationService;
        private readonly IRedisCacheService cacheService;

        public ReservationController(IReservationService reservationService, IRedisCacheService cacheService)
        {
            this.reservationService = reservationService;
            this.cacheService = cacheService;
        }

        [HttpGet("getByUserId")]
        public IActionResult GetByUserId(int userId)
        {
            if (cacheService.IsConnected())
            {
                var reservedBookFromCache = cacheService.GetAll<ReservationDTOGet>("*reserved:user" + userId + ":*");
                return Ok(reservedBookFromCache);
            }

            var reservedBooksFromDb = reservationService.GetReservationsByUserId(userId);
            return Ok(reservedBooksFromDb);
        }


        [HttpPost("add")]
        public async Task<IActionResult> AddReservation(ReservationDTO dto)
        {
            var result = await reservationService.AddReservationAsync(dto);

            if (result)
            {
                // todo: eger redis ayakta degil ise rezerve edilen kitap cache'den nasil getirilecek?
                if (cacheService.IsConnected())
                {
                    ReservationDTOGet reservationDetail = reservationService.GetReservationDetailForCache(dto.UserID, dto.BookID);
                    cacheService.Add("reserved:user" + dto.UserID + ":kitap" + dto.BookID, reservationDetail);
                }
                return Ok("kayit basarili");
            }
            return BadRequest("aynı kitap rezervasyon edilmis");
        }


        [HttpPost("delete")]
        public IActionResult DeleteReservation(ReservationDeleteDTO dto)
        {
            var result = reservationService.DeleteReservation(dto.UserId, dto.BookId);

            if (result)
            {
                // todo: redis calismiyorken silinen rezervasyon, redis geldiğinde hala gösteriliyor olacak. Nasıl çözebiliriz?
                if(cacheService.IsConnected())
                {
                    cacheService.Remove("reserved:user" + dto.UserId + ":kitap" + dto.BookId);
                }
                return Ok("silme basarili");
            }

            return BadRequest("reservasyon bulunamadi");
        }
    }
}
