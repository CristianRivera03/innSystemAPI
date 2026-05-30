using InnSystem.BLL.Services.Contract;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using InnSystem.DAL.Repositories.Contract;
using InnSystem.Model;
using Microsoft.EntityFrameworkCore;

namespace InnSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IGenericRepository<Booking> _bookingRepo;

        public PaymentController(IConfiguration config, IGenericRepository<Booking> bookingRepo)
        {
            _config = config;
            _bookingRepo = bookingRepo;
        }

        [HttpPost("wompi-link/{bookingId}")]
        public async Task<IActionResult> GenerateWompiLink(Guid bookingId)
        {
            try
            {
                var booking = await _bookingRepo.Query().Include(b => b.IdRoomNavigation).FirstOrDefaultAsync(b => b.IdBooking == bookingId);
                if (booking == null) return NotFound("Reserva no encontrada");

                var appId = _config["Wompi:AppId"];
                var apiSecret = _config["Wompi:ApiSecret"];
                var envUrl = _config["Wompi:Environment"] ?? "https://api.wompi.sv/EnlacePago";

                using var client = new HttpClient();
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiSecret}");

                // Wompi SV payload
                var payload = new
                {
                    identificadorEnlaceComercio = booking.IdBooking.ToString(),
                    monto = booking.TotalCost,
                    nombreProducto = $"Reserva Habitación {booking.IdRoomNavigation.RoomNumber}",
                    cantidadPermitida = 1,
                    urlRedireccion = "http://localhost:4200/bookings"
                };

                var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
                var response = await client.PostAsync(envUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    var responseData = await response.Content.ReadAsStringAsync();
                    // Assuming the response contains urlEnlace
                    return Ok(responseData);
                }

                var error = await response.Content.ReadAsStringAsync();
                return BadRequest($"Error de Wompi: {error}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
