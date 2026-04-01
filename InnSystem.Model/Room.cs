using System;
using System.Collections.Generic;

namespace InnSystem.Model;

public partial class Room
{
    public int IdRoom { get; set; }

    public string RoomNumber { get; set; } = null!;

    public string RoomType { get; set; } = null!;

    public string? Description { get; set; }

    public decimal BasePrice { get; set; }

    public int GuestCapacity { get; set; }

    public string? OperationalStatus { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual ICollection<RoomImage> RoomImages { get; set; } = new List<RoomImage>();

    public virtual ICollection<Service> IdServices { get; set; } = new List<Service>();
}
