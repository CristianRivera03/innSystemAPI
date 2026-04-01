using System;
using System.Collections.Generic;

namespace InnSystem.Model;

public partial class Role
{
    public int IdRole { get; set; }

    public string RoleName { get; set; } = null!;

    public virtual ICollection<User> Users { get; set; } = new List<User>();

    public virtual ICollection<Module> IdModules { get; set; } = new List<Module>();
}
