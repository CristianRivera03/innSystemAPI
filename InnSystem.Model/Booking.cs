using System;
using System.Collections.Generic;

namespace InnSystem.Model;

public partial class Booking
{
    public Guid IdBooking { get; set; }

    public Guid IdUser { get; set; }

    public int IdRoom { get; set; }

    public DateOnly CheckIn { get; set; }

    public DateOnly CheckOut { get; set; }

    public int GuestsCount { get; set; }

    public decimal TotalCost { get; set; }

    public string? CancelReason { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public int IdStatus { get; set; }

    public virtual Room IdRoomNavigation { get; set; } = null!;

    public virtual BookingStatus IdStatusNavigation { get; set; } = null!;

    public virtual User IdUserNavigation { get; set; } = null!;

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
