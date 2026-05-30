using System.ComponentModel.DataAnnotations;

namespace InnSystem.DTO.Rooms
{
    public class ServiceCreateDTO
    {
        [Required]
        public string Name { get; set; } = null!;
    }
}
