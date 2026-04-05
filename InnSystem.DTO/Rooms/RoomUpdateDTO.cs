namespace InnSystem.DTO.Rooms
{
    public class RoomUpdateDTO
    {
        public string RoomType { get; set; } = null!;
        public string? Description { get; set; }
        public decimal BasePrice { get; set; }
        public int GuestCapacity { get; set; }
        public string? OperationalStatus { get; set; }
    }
}
