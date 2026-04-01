using System;
using System.Collections.Generic;

namespace InnSystem.Model;

public partial class PaymentMethod
{
    public int IdMethod { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
