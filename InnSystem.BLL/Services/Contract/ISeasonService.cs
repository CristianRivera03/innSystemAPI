using InnSystem.DTO.Catalogs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InnSystem.BLL.Services.Contract
{
    public interface ISeasonService
    {
        Task<List<SeasonDTO>> GetAllAsync();
        Task<SeasonDTO> GetByIdAsync(int id);
        Task<SeasonDTO> CreateAsync(SeasonCreateDTO model);
        Task<SeasonDTO> UpdateAsync(int id, SeasonUpdateDTO model);
        Task<bool> InactivateAsync(int id);
    }
}
