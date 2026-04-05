using System;

namespace InnSystem.DTO.Payments
{
    public class PaymentCreateDTO
    {
        public Guid IdBooking { get; set; }
        public int IdMethod { get; set; }
        public int IdType { get; set; }
        public int IdStatus { get; set; }
        public decimal Amount { get; set; }
        public string? ExternalRef { get; set; }
    }
}
