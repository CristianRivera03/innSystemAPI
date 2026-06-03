using InnSystem.BLL.Services.Contract;
using InnSystem.DAL.Repositories.Contract;
using InnSystem.DTO.Wompi;
using InnSystem.Model;
using InnSystem.Utility.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace InnSystem.BLL.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IGenericRepository<Payment> _paymentRepo;
        private readonly IGenericRepository<Booking> _bookingRepo;
        private readonly IPdfService _pdfService;
        private readonly IEmailService _emailService;

        public PaymentService(
            IGenericRepository<Payment> paymentRepo, 
            IGenericRepository<Booking> bookingRepo,
            IPdfService pdfService,
            IEmailService emailService)
        {
            _paymentRepo = paymentRepo;
            _bookingRepo = bookingRepo;
            _pdfService = pdfService;
            _emailService = emailService;
        }

        public async Task<bool> ProcessWompiWebhookAsync(WompiWebhookDTO webhookData)
        {
            // Wompi SV sends IdExterno (our bookingId) or EnlacePago.IdentificadorEnlaceComercio
            string referencia = !string.IsNullOrEmpty(webhookData.IdExterno) 
                                ? webhookData.IdExterno 
                                : webhookData.EnlacePago?.IdentificadorEnlaceComercio ?? string.Empty;

            if (string.IsNullOrEmpty(referencia))
            {
                // Si no hay referencia, no podemos ligarlo a un Booking
                return false;
            }

            if (!Guid.TryParse(referencia, out Guid bookingId))
            {
                return false; // Referencia no es un UUID válido
            }

            var query =  _bookingRepo.Query()
                            .Include(b => b.IdUserNavigation)
                            .Include(b => b.IdRoomNavigation);
            var booking = await query.FirstOrDefaultAsync(b => b.IdBooking == bookingId);

            if (booking == null) return false;

            // Wompi devuelve el monto en centavos a veces, pero asumiendo que lo manda en el formato estándar decimal
            // Ojo: Si el estado es APROBADA
            if (webhookData.ResultadoTransaccion.ToUpper() == "EXITOSAAPROBADA" || webhookData.ResultadoTransaccion.ToUpper() == "APROBADA")
            {
                // Cambiar estado a Confirmada (IdStatus = 2) o Completada (IdStatus = 3)
                // Asumimos 2 = Confirmada
                booking.IdStatus = 2;
                await _bookingRepo.Update(booking);

                // Parse amount from string like "126.00"
                decimal.TryParse(webhookData.Monto, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out decimal amount);

                // Registrar en la tabla Payment
                var newPayment = new Payment
                {
                    IdBooking = bookingId,
                    Amount = amount,
                    ExternalRef = webhookData.IdTransaccion,
                    PaymentDate = DateTime.UtcNow,
                    IdStatus = 1, // Asumir 1 = Completado en PaymentStatus
                    IdMethod = 1  // Asumir 1 = Tarjeta / Wompi en PaymentMethod
                };

                await _paymentRepo.Create(newPayment);

                try
                {
                    // Generar PDF y enviar correo
                    string customerName = booking.IdUserNavigation != null ? $"{booking.IdUserNavigation.FirstName} {booking.IdUserNavigation.LastName}" : "Cliente";
                    string customerEmail = booking.IdUserNavigation?.Email ?? "";

                    if (!string.IsNullOrEmpty(customerEmail))
                    {
                        var pdfBytes = _pdfService.GenerateInvoicePdf(booking, newPayment, customerName, customerEmail);
                        string subject = $"Confirmación de Pago y Factura - Reserva Hab. {booking.IdRoomNavigation?.RoomNumber}";
                        string body = $"<p>Hola {customerName},</p><p>Adjuntamos la factura correspondiente a su reciente pago por la reserva de la habitación {booking.IdRoomNavigation?.RoomNumber}.</p><p>Gracias por elegir InnSystem Hotel.</p>";
                        
                        await _emailService.SendEmailAsync(customerEmail, subject, body, pdfBytes, $"Factura_{newPayment.ExternalRef}.pdf");
                    }
                }
                catch (Exception ex)
                {
                    // Log email error but don't fail the webhook processing
                    System.Console.WriteLine($"Error enviando correo de factura: {ex.Message}");
                }

                return true;
            }

            return true;
        }
    }
}
