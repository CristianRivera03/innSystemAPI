using System;
using System.Collections.Generic;

namespace InnSystem.Model;

public partial class User
{
    public Guid IdUser { get; set; }

    public int IdRole { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? Phone { get; set; }

    public string? DocumentId { get; set; }

    public string PasswordHash { get; set; } = null!;

    public bool? IsActive { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual ICollection<ActionLog> ActionLogs { get; set; } = new List<ActionLog>();

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual Role IdRoleNavigation { get; set; } = null!;
}
