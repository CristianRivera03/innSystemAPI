using System;
using System.ComponentModel.DataAnnotations;

namespace InnSystem.DTO.Catalogs
{
    public class SeasonUpdateDTO
    {
        [Required]
        public int IdSeason { get; set; }
        [Required]
        public string SeasonName { get; set; } = null!;
        [Required]
        public DateOnly StartDate { get; set; }
        [Required]
        public DateOnly EndDate { get; set; }
        [Required]
        [Range(0.01, 10.0)]
        public decimal PriceMultiplier { get; set; }
    }
}
