using InnSystem.DTO.Rooms;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InnSystem.BLL.Services.Contract
{
    public interface IServiceManagerService
    {
        Task<List<ServiceDTO>> GetAllAsync();
        Task<ServiceDTO> GetByIdAsync(int id);
        Task<ServiceDTO> CreateAsync(ServiceCreateDTO model);
        Task<ServiceDTO> UpdateAsync(int id, ServiceUpdateDTO model);
        Task<bool> InactivateAsync(int id);
    }
}
