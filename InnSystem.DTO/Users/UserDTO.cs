using System;

namespace InnSystem.DTO.Users
{
    public class UserDTO
    {
        public Guid IdUser { get; set; }
        public int IdRole { get; set; }
        public string? RoleName { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? Phone { get; set; }
        public string? DocumentId { get; set; }
        public bool IsActive { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
