using System;

namespace InnSystem.DTO.Users
{
    public class UserCreateDTO
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string? Phone { get; set; }
        public string? DocumentId { get; set; }
    }
}
