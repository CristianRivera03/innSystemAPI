using InnSystem.DTO.Wompi;

namespace InnSystem.BLL.Services.Contract
{
    public interface IPaymentService
    {
        Task<bool> ProcessWompiWebhookAsync(WompiWebhookDTO webhookData);
    }
}
