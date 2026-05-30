using System;
using System.ComponentModel.DataAnnotations;

namespace InnSystem.DTO.Catalogs
{
    public class RoomTypeUpdateDTO
    {
        [Required]
        public int IdRoomType { get; set; }
        [Required]
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        [Required]
        public int GuestCapacity { get; set; }
    }
}
