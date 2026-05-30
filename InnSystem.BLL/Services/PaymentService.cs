using InnSystem.BLL.Services.Contract;
using InnSystem.DAL.Repositories.Contract;
using InnSystem.DTO.Wompi;
using InnSystem.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace InnSystem.BLL.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IGenericRepository<Payment> _paymentRepo;
        private readonly IGenericRepository<Booking> _bookingRepo;

        public PaymentService(IGenericRepository<Payment> paymentRepo, IGenericRepository<Booking> bookingRepo)
        {
            _paymentRepo = paymentRepo;
            _bookingRepo = bookingRepo;
        }

        public async Task<bool> ProcessWompiWebhookAsync(WompiWebhookDTO webhookData)
        {
            var tx = webhookData.Data.Transaccion;

            // La referencia suele estar en el campo Referencia o en IdentificadorEnlaceComercio (EnlacePago)
            string referencia = !string.IsNullOrEmpty(tx.Referencia) 
                                ? tx.Referencia 
                                : webhookData.Data.EnlacePago?.IdentificadorEnlaceComercio ?? string.Empty;

            if (string.IsNullOrEmpty(referencia))
            {
                // Si no hay referencia, no podemos ligarlo a un Booking
                return false;
            }

            if (!Guid.TryParse(referencia, out Guid bookingId))
            {
                return false; // Referencia no es un UUID válido
            }

            var query =  _bookingRepo.Query();
            var booking = await query.FirstOrDefaultAsync(b => b.IdBooking == bookingId);

            if (booking == null) return false;

            // Wompi devuelve el monto en centavos a veces, pero asumiendo que lo manda en el formato estándar decimal
            // Ojo: Si el estado es APROBADA
            if (tx.Estado.ToUpper() == "APPROVED" || tx.Estado.ToUpper() == "APROBADA")
            {
                // Cambiar estado a Confirmada (IdStatus = 2) o Completada (IdStatus = 3)
                // Asumimos 2 = Confirmada
                booking.IdStatus = 2;
                await _bookingRepo.Update(booking);

                // Registrar en la tabla Payment
                var newPayment = new Payment
                {
                    IdBooking = bookingId,
                    Amount = tx.Monto,
                    ExternalRef = tx.IdTransaccion,
                    PaymentDate = DateTime.Now,
                    IdStatus = 1, // Asumir 1 = Completado en PaymentStatus
                    IdMethod = 1  // Asumir 1 = Tarjeta / Wompi en PaymentMethod
                };

                await _paymentRepo.Create(newPayment);
                return true;
            }

            return true;
        }
    }
}
