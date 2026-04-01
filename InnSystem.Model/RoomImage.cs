using System;
using System.Collections.Generic;

namespace InnSystem.Model;

public partial class RoomImage
{
    public int IdImage { get; set; }

    public int IdRoom { get; set; }

    public string Url { get; set; } = null!;

    public string? Description { get; set; }

    public virtual Room IdRoomNavigation { get; set; } = null!;
}
