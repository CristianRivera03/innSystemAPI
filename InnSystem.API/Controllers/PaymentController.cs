using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using InnSystem.DAL.Repositories.Contract;
using InnSystem.Model;
using Microsoft.EntityFrameworkCore;
using InnSystem.Utility.Interfaces;

namespace InnSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IGenericRepository<Booking> _bookingRepo;
        private readonly IEmailService _emailService;

        public PaymentController(IConfiguration config, IGenericRepository<Booking> bookingRepo, IEmailService emailService)
        {
            _config = config;
            _bookingRepo = bookingRepo;
            _emailService = emailService;
        }

        [HttpPost("wompi-link/{bookingId}")]
        public async Task<IActionResult> GenerateWompiLink(Guid bookingId)
        {
            try
            {
                var booking = await _bookingRepo.Query()
                    .Include(b => b.IdRoomNavigation)
                    .FirstOrDefaultAsync(b => b.IdBooking == bookingId);

                if (booking == null)
                    return NotFound("Reserva no encontrada.");

                var apiSecret = _config["Wompi:ApiSecret"];
                var appId = _config["Wompi:AppId"];

                using var client = new HttpClient();

                // Paso 1: OAuth 2.0 - Obtener access_token con Client Credentials
                var tokenParams = new Dictionary<string, string>
                {
                    { "grant_type", "client_credentials" },
                    { "audience", "wompi_api" },
                    { "client_id", appId! },
                    { "client_secret", apiSecret! }
                };

                var tokenResponse = await client.PostAsync(
                    "https://id.wompi.sv/connect/token",
                    new FormUrlEncodedContent(tokenParams)
                );

                var tokenBody = await tokenResponse.Content.ReadAsStringAsync();

                if (!tokenResponse.IsSuccessStatusCode)
                    return BadRequest(new { error = "No se pudo obtener token de Wompi", detail = tokenBody });

                using var tokenDoc = JsonDocument.Parse(tokenBody);
                var accessToken = tokenDoc.RootElement.GetProperty("access_token").GetString();

                // Paso 2: Crear enlace de pago con el access_token
                client.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

                var payload = new
                {
                    identificadorEnlaceComercio = booking.IdBooking.ToString(),
                    monto = booking.TotalCost,
                    nombreProducto = $"Reserva Hab. {booking.IdRoomNavigation.RoomNumber}",
                    cantidadPermitida = 1,
                    enlaceConfiguracion = new
                    {
                        urlRedirect = $"http://localhost:4200/payment-return?bookingId={booking.IdBooking}",
                        esMontoEditable = false
                    }
                };

                var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
                var response = await client.PostAsync("https://api.wompi.sv/EnlacePago", content);

                var responseBody = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    // Wompi SV responde con { idEnlace, urlQrCodeEnlace, urlEnlace, estaProductivo }
                    using var doc = JsonDocument.Parse(responseBody);
                    var urlEnlace = doc.RootElement.TryGetProperty("urlEnlace", out var urlProp)
                        ? urlProp.GetString()
                        : null;

                    return Ok(new { urlEnlace });
                }

                return BadRequest(new { wompiError = responseBody, statusCode = (int)response.StatusCode });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Llamado por el frontend desde /payment-return tras el redirect de Wompi.
        /// Marca la reserva como Confirmada (IdStatus = 2) solo si aún está Pendiente.
        /// </summary>
        [HttpPost("confirm-from-redirect")]
        public async Task<IActionResult> ConfirmFromRedirect([FromQuery] string bookingId, [FromQuery] string? idTransaccion)
        {
            if (string.IsNullOrWhiteSpace(bookingId) || !Guid.TryParse(bookingId, out Guid bookingGuid))
                return BadRequest(new { status = false, msg = "bookingId inválido o ausente." });

            try
            {
                var booking = await _bookingRepo.Query()
                    .FirstOrDefaultAsync(b => b.IdBooking == bookingGuid);

                if (booking == null)
                    return NotFound(new { status = false, msg = "Reserva no encontrada." });

                // Solo confirmar si aún está en Pendiente (evita retroceder estados)
                if (booking.IdStatus == 1)
                {
                    booking.IdStatus = 2; // Confirmada
                    await _bookingRepo.Update(booking);
                }

                return Ok(new
                {
                    status = true,
                    msg = "Reserva confirmada.",
                    idTransaccion,
                    currentStatus = booking.IdStatus
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { status = false, msg = ex.Message });
            }
        }

        [HttpPost("test-email")]
        public async Task<IActionResult> TestEmail([FromQuery] string toEmail)
        {
            try
            {
                await _emailService.SendEmailAsync(
                    toEmail, 
                    "Correo de Prueba - InnSystem", 
                    "<h1>Prueba Exitosa</h1><p>Este es un correo de prueba usando System.Net.Mail.</p>", 
                    null, 
                    null);
                return Ok(new { status = true, msg = "Correo enviado exitosamente." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { status = false, msg = ex.Message });
            }
        }
    }
}

