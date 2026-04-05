using System;
using System.Collections.Generic;
using System.Text;

namespace InnSystem.DTO.Roles
{
    public class ModuleDTO
    {

        public int IdModule { get; set; }

        public string Name { get; set; } = null!;

        public string FrontendPath { get; set; } = null!;

        public string? Icon { get; set; }

    }
}
