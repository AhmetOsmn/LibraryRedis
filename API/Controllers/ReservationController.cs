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

        [HttpGet("getall")]
        public IActionResult GetAll()
        {
            var reservations = reservationService.GetReservations();
            
            if(reservations.Count() > 0) return Ok(reservations);

            return BadRequest("Listelenecek rezervasyon yok");  
        }


        [HttpGet("getByUserId")]
        public IActionResult GetByUserId(int userId)
        {
            var reservedBookFromCache = cacheService.GetAll<ReservationDTOGet>("reserved:username:");
                                                                                                   
            return Ok(reservedBookFromCache);            
        }


        [HttpPost("add")]
        public async Task<IActionResult> AddReservation(ReservationDTO dto)
        {
            var result = await reservationService.AddReservationAsync(dto);

            if (result) return Ok("kayit basarili");
            return BadRequest("aynı kitap rezervasyon edilmiş");
        }


        [HttpPost("delete")]
        public IActionResult DeleteReservation(int reservationID)
        {
            var result = reservationService.DeleteReservation(reservationID);

            if (result) return Ok("silme basarili");
            return BadRequest("reservasyon bulunamadi");
        }
    }
}
