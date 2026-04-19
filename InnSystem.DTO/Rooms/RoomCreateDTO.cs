namespace InnSystem.DTO.Rooms
{
    public class RoomCreateDTO
    {
        public string RoomNumber { get; set; } = null!;
        public int IdRoomType { get; set; }
        public int IdStatus { get; set; }
        public string? Description { get; set; }
    }
}
