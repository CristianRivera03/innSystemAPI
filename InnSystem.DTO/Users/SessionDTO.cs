using InnSystem.DTO.Roles;
using System;
using System.Collections.Generic;
using System.Text;

namespace InnSystem.DTO.Users
{
    public class SessionDTO
    {


        public Guid IdUser { get; set; }

        public string? RoleName { get; set; }

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string Email { get; set; } = null!;
        public List<ModuleDTO> AllowedModules { get; set; } = null!;

    }
}
