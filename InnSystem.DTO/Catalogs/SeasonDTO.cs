using System;

namespace InnSystem.DTO.Catalogs
{
    public class SeasonDTO
    {
        public int IdSeason { get; set; }
        public string SeasonName { get; set; } = null!;
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public decimal PriceMultiplier { get; set; }
    }
}
