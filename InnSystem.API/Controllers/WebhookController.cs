using InnSystem.BLL.Services.Contract;
using InnSystem.DTO.Wompi;
using Microsoft.AspNetCore.Mvc;

namespace InnSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WebhookController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public WebhookController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpPost("wompi")]
        public async Task<IActionResult> ReceiveWompiWebhook([FromBody] System.Text.Json.JsonElement payload)
        {
            var jsonString = payload.ToString();
            System.Console.WriteLine("---------------- WOMPI WEBHOOK RECEIVED ----------------");
            System.Console.WriteLine(jsonString);
            System.Console.WriteLine("---------------------------------------------------------");

            try
            {
                var webhookData = System.Text.Json.JsonSerializer.Deserialize<WompiWebhookDTO>(
                    jsonString, 
                    new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                );

                if (webhookData == null || string.IsNullOrEmpty(webhookData.IdTransaccion))
                    return BadRequest("Payload inválido.");

                var success = await _paymentService.ProcessWompiWebhookAsync(webhookData);
                if (success)
                    return Ok();
                else
                {
                    System.Console.WriteLine("ProcessWompiWebhookAsync retornó false.");
                    return BadRequest("Error procesando el webhook.");
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"Webhook Error: {ex.Message}");
                return StatusCode(500, ex.Message);
            }
        }
    }
}
