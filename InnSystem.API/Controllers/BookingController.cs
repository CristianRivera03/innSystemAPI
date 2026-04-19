using InnSystem.API.Utility;
using InnSystem.BLL.Services.Contract;
using InnSystem.DAL.Repositories.Contract;
using InnSystem.DTO.Bookings;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InnSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _bookingService;

        public BookingController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        [HttpGet]
        [Route("Get")]
        public async Task<IActionResult> Get()
        {
            var rsp = new Response<List<BookingDTO>>();
            try
            {
                rsp.status = true;
                rsp.value = await _bookingService.GetAllAsync();

            }
            catch(Exception ex)
            {
                rsp.status = false;
                rsp.msg = ex.Message;
            }
            return Ok(rsp);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var rsp = new Response<BookingDTO>();
            try
            {
                var booking = await _bookingService.GetByIdAsync(id);
                if (booking == null)
                {
                    rsp.status = false;
                    rsp.msg = "Booking no encontrada";
                    return NotFound(rsp);
                }
                rsp.status = true;
                rsp.value = booking;
            }
            catch (Exception ex)
            {
                rsp.status = false;
                rsp.msg = ex.Message;
                return StatusCode(500, rsp);
            }
            return Ok(rsp);
        }

        [HttpPatch("{id}/status")]
        public async Task<IActionResult> ChangeStatus(Guid id, [FromBody] int statusId)
        {
            var rsp = new Response<bool>();
            try
            {
                rsp.status = await _bookingService.ChangeStatusAsync(id, statusId);
                rsp.value = rsp.status;
                if (!rsp.status) return NotFound(rsp);
            }
            catch (Exception ex)
            {
                rsp.status = false;
                rsp.msg = ex.Message;
                return StatusCode(500, rsp);
            }
            return Ok(rsp);
        }
    }
}
