using System.Collections.Generic;

namespace InnSystem.DTO.Roles
{
    public class AssignModulesDTO
    {
        public int IdRole { get; set; }
        public List<int> ModuleIds { get; set; } = new List<int>();
    }
}
