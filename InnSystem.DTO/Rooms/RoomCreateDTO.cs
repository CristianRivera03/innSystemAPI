using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace InnSystem.DTO.Rooms
{
    public class RoomCreateDTO
    {
        public string RoomNumber { get; set; } = null!;
        public int IdRoomType { get; set; }
        public int IdStatus { get; set; }
        public string? Description { get; set; }
        public decimal BasePrice { get; set; }

        public List<int>? ServiceIds { get; set; }
        public List<IFormFile>? Photographs { get; set; }
    }
}
