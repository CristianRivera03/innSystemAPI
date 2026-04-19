using System;
using System.Collections.Generic;

namespace InnSystem.Model;

public partial class RoomType
{
    public int IdRoomType { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public decimal BasePrice { get; set; }

    public int GuestCapacity { get; set; }

    public virtual ICollection<Room> Rooms { get; set; } = new List<Room>();
}
