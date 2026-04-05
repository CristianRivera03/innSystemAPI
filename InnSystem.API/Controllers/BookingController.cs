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
    }
}
