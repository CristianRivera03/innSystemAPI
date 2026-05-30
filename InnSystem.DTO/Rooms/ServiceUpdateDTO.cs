using System.ComponentModel.DataAnnotations;

namespace InnSystem.DTO.Rooms
{
    public class ServiceUpdateDTO
    {
        [Required]
        public int IdService { get; set; }
        
        [Required]
        public string Name { get; set; } = null!;
    }
}
