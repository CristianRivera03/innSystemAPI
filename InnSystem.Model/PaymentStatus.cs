using System;
using System.Collections.Generic;

namespace InnSystem.Model;

public partial class PaymentStatus
{
    public int IdStatus { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
