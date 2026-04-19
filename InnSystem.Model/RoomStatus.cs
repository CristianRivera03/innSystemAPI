using System;
using System.Collections.Generic;

namespace InnSystem.Model;

public partial class RoomStatus
{
    public int IdStatus { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Room> Rooms { get; set; } = new List<Room>();
}
