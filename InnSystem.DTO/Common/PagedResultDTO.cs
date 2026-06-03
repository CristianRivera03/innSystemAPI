using System.Collections.Generic;

namespace InnSystem.DTO.Common
{
    public class PagedResultDTO<T>
    {
        public List<T> Items { get; set; } = new List<T>();
        public int TotalItems { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
    }
}
