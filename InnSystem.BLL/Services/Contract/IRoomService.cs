using System;
using System.Collections.Generic;
using System.Text;

using InnSystem.DTO;
using InnSystem.DTO.Roles;
using InnSystem.DTO.Rooms;

namespace InnSystem.BLL.Services.Contract
{
    public interface IRoomService
    {
        Task<List<RoomDTO>> GetAllAsync();
        Task<RoomDTO> GetByIdAsync(int id );
        Task<RoomDTO> CreateAsync(RoomCreateDTO model);
        Task<bool> UpdateAsync(int id ,RoomUpdateDTO model);
        Task<bool> SoftDelete(int id);
        Task<bool> HardDelete(int id);

        Task<List<RoomDTO>> GetAvailableRoomsAsync(DateTime checkIn, DateTime checkOut, int guestsCount);
        Task<bool> ChangeOperationalStatusAsync(int id, int statusId);

    }
}
