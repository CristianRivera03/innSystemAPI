using System;

namespace InnSystem.DTO.Users
{
    public class UserChangeRoleDTO
    {
        public Guid IdUser { get; set; }
        public int IdRole { get; set; }
    }
}
