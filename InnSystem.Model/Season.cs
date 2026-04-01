using System;
using System.Collections.Generic;

namespace InnSystem.Model;

public partial class Season
{
    public int IdSeason { get; set; }

    public string SeasonName { get; set; } = null!;

    public DateOnly StartDate { get; set; }

    public DateOnly EndDate { get; set; }

    public decimal PriceMultiplier { get; set; }
}
