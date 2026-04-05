using System;

namespace InnSystem.DTO.Bookings
{
    public class BookingCreateDTO
    {
        public Guid IdUser { get; set; }
        public int IdRoom { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public int GuestsCount { get; set; }
    }
}
