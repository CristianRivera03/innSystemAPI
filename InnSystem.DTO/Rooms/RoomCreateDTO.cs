namespace InnSystem.DTO.Rooms
{
    public class RoomCreateDTO
    {
        public string RoomNumber { get; set; } = null!;
        public string RoomType { get; set; } = null!;
        public string? Description { get; set; }
        public decimal BasePrice { get; set; }
        public int GuestCapacity { get; set; }
        // OperationalStatus should probably default to 'Active' upon creation, handled by DB/service.
    }
}
