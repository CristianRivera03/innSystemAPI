using System;
using System.Collections.Generic;

namespace InnSystem.Model;

public partial class Module
{
    public int IdModule { get; set; }

    public string Name { get; set; } = null!;

    public string FrontendPath { get; set; } = null!;

    public string? Icon { get; set; }

    public bool? IsActive { get; set; }

    public virtual ICollection<Role> IdRoles { get; set; } = new List<Role>();
}
