using System;
using System.Collections.Generic;

namespace InnSystem.Model;

public partial class BookingStatus
{
    public int IdStatus { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}
