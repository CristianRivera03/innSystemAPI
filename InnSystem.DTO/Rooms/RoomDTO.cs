using System;

namespace InnSystem.DTO.Rooms
{
    public class RoomDTO
    {
        public int IdRoom { get; set; }
        public string RoomNumber { get; set; } = null!;
        public int? IdRoomType { get; set; }
        public string RoomType { get; set; } = null!;
        public string? Description { get; set; }
        public decimal BasePrice { get; set; }
        public int GuestCapacity { get; set; }
        public int IdStatus { get; set; }
        public string? OperationalStatus { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
