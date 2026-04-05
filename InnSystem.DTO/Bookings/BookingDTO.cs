using System;

namespace InnSystem.DTO.Bookings
{
    public class BookingDTO
    {
        public Guid IdBooking { get; set; }
        public Guid IdUser { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string DocumentId { get; set; } = null!;
        public int IdRoom { get; set; }
        public DateOnly CheckIn { get; set; }
        public DateOnly CheckOut { get; set; }
        public int GuestsCount { get; set; }
        public decimal TotalCost { get; set; }
        public string? Status { get; set; }
        public string? CancelReason { get; set; }
    }
}
