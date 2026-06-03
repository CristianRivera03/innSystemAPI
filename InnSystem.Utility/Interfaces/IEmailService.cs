using System.Threading.Tasks;

namespace InnSystem.Utility.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(string toEmail, string subject, string body, byte[] attachmentData, string attachmentName);
    }
}
