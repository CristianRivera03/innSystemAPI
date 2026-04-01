using System;
using System.Collections.Generic;
using System.Net;

namespace InnSystem.Model;

public partial class ActionLog
{
    public Guid IdLog { get; set; }

    public Guid? IdUser { get; set; }

    public string Action { get; set; } = null!;

    public string AffectedTable { get; set; } = null!;

    public string RecordId { get; set; } = null!;

    public string? Details { get; set; }

    public IPAddress? SourceIp { get; set; }

    public DateTime? ActionDate { get; set; }

    public virtual User? IdUserNavigation { get; set; }
}
