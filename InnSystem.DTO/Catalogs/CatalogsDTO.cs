using System;
using System.Collections.Generic;
using System.Text;

namespace InnSystem.DTO.Catalogs
{
    public class StatusDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class RoomTypeDTO
    {
        public int IdRoomType { get; set; }
        public string Name { get; set; }
        public decimal BasePrice { get; set; }
        public int GuestCapacity { get; set; }
    }

    public class CatalogDTO
    {
        public List<RoomTypeDTO> RoomTypes { get; set; }
        public List<StatusDTO> RoomStatuses { get; set; }
    }
}
