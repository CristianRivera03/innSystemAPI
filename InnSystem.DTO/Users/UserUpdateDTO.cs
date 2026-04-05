using System;

namespace InnSystem.DTO.Users
{
    public class UserUpdateDTO
    {
        public int IdRole { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string? Phone { get; set; }
        public string? DocumentId { get; set; }
        public bool IsActive { get; set; }
    }
}
