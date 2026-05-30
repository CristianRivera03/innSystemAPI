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
        public async Task<IActionResult> ReceiveWompiWebhook([FromBody] WompiWebhookDTO webhookData)
        {
            if (webhookData == null || webhookData.Data == null)
                return BadRequest("Payload inválido.");

            try
            {
                var success = await _paymentService.ProcessWompiWebhookAsync(webhookData);
                if (success)
                    return Ok();
                else
                    return BadRequest("Error procesando el webhook.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
