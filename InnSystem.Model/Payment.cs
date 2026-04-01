using System;
using System.Collections.Generic;

namespace InnSystem.Model;

public partial class Payment
{
    public Guid IdPayment { get; set; }

    public Guid IdBooking { get; set; }

    public int? IdMethod { get; set; }

    public int? IdType { get; set; }

    public int? IdStatus { get; set; }

    public decimal Amount { get; set; }

    public string? ExternalRef { get; set; }

    public DateTime? PaymentDate { get; set; }

    public virtual Booking IdBookingNavigation { get; set; } = null!;

    public virtual PaymentMethod? IdMethodNavigation { get; set; }

    public virtual PaymentStatus? IdStatusNavigation { get; set; }

    public virtual InvoiceType? IdTypeNavigation { get; set; }
}
