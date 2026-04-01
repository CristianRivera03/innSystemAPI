using System;
using System.Collections.Generic;

namespace InnSystem.Model;

public partial class Service
{
    public int IdService { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Room> IdRooms { get; set; } = new List<Room>();
}
