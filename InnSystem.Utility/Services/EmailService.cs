using InnSystem.Utility.Interfaces;
using InnSystem.Utility.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace InnSystem.Utility.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IOptions<EmailSettings> options, ILogger<EmailService> logger)
        {
            _emailSettings = options.Value;
            _logger = logger;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body, byte[] attachmentData, string attachmentName)
        {
            if (string.IsNullOrWhiteSpace(_emailSettings.Host))
                throw new InvalidOperationException("La configuración del servidor SMTP está incompleta.");
            if (string.IsNullOrWhiteSpace(_emailSettings.Email))
                throw new InvalidOperationException("La dirección de correo del remitente está vacía.");
            if (string.IsNullOrWhiteSpace(_emailSettings.Password))
                throw new InvalidOperationException("La contraseña de aplicación está vacía.");
            if (string.IsNullOrWhiteSpace(toEmail))
                throw new ArgumentException("La dirección de correo del destinatario está vacía.");

            if (!MailAddress.TryCreate(_emailSettings.Email, out _))
                throw new FormatException($"La dirección del remitente '{_emailSettings.Email}' no tiene un formato válido.");
            if (!MailAddress.TryCreate(toEmail, out _))
                throw new FormatException($"La dirección de destino '{toEmail}' no tiene un formato válido.");

            try
            {
                _logger.LogInformation("Enviando correo. Remitente: '{Remitente}', Destinatario: '{Destinatario}'", _emailSettings.Email, toEmail);

                using (var client = new SmtpClient(_emailSettings.Host, _emailSettings.Port))
                {
                    client.EnableSsl = true;
                    client.Credentials = new NetworkCredential(_emailSettings.Email, _emailSettings.Password);

                    var mailMessage = new MailMessage
                    {
                        From = new MailAddress(_emailSettings.Email, "InnSystem Hotel"),
                        Subject = subject,
                        Body = body,
                        IsBodyHtml = true
                    };
                    mailMessage.To.Add(toEmail);

                    if (attachmentData != null && attachmentData.Length > 0)
                    {
                        var stream = new MemoryStream(attachmentData);
                        mailMessage.Attachments.Add(new Attachment(stream, attachmentName, "application/pdf"));
                    }

                    await client.SendMailAsync(mailMessage);
                    _logger.LogInformation("Correo enviado exitosamente a {Destinatario} con asunto '{Asunto}'", toEmail, subject);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al enviar correo a {Destinatario}", toEmail);
                throw;
            }
        }
    }
}
