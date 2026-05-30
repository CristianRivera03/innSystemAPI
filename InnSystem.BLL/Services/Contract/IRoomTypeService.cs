using InnSystem.DTO.Catalogs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InnSystem.BLL.Services.Contract
{
    public interface IRoomTypeService
    {
        Task<List<RoomTypeDTO>> GetAllAsync();
        Task<RoomTypeDTO> GetByIdAsync(int id);
        Task<RoomTypeDTO> CreateAsync(RoomTypeCreateDTO model);
        Task<RoomTypeDTO> UpdateAsync(int id, RoomTypeUpdateDTO model);
        Task<bool> InactivateAsync(int id);
    }
}
