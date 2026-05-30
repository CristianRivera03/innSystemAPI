using AutoMapper;
using InnSystem.BLL.Services.Contract;
using InnSystem.DAL.Repositories.Contract;
using InnSystem.DTO.Catalogs;
using InnSystem.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InnSystem.BLL.Services
{
    public class SeasonService : ISeasonService
    {
        private readonly IGenericRepository<Season> _repository;
        private readonly IMapper _mapper;

        public SeasonService(IGenericRepository<Season> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<SeasonDTO>> GetAllAsync()
        {
            try
            {
                var query = _repository.Query(r => r.DeletedAt == null);
                return _mapper.Map<List<SeasonDTO>>(await query.ToListAsync());
            }
            catch
            {
                throw;
            }
        }

        public async Task<SeasonDTO> GetByIdAsync(int id)
        {
            try
            {
                var query = _repository.Query(r => r.IdSeason == id && r.DeletedAt == null);
                var season = await query.FirstOrDefaultAsync();
                
                if (season == null)
                    throw new Exception("La temporada no fue encontrada.");

                return _mapper.Map<SeasonDTO>(season);
            }
            catch
            {
                throw;
            }
        }

        public async Task<SeasonDTO> CreateAsync(SeasonCreateDTO model)
        {
            try
            {
                var dbModel = _mapper.Map<Season>(model);
                var created = await _repository.Create(dbModel);
                if (created.IdSeason == 0)
                    throw new Exception("No se pudo crear la temporada.");

                return _mapper.Map<SeasonDTO>(created);
            }
            catch
            {
                throw;
            }
        }

        public async Task<SeasonDTO> UpdateAsync(int id, SeasonUpdateDTO model)
        {
            try
            {
                var query = _repository.Query(r => r.IdSeason == id && r.DeletedAt == null);
                var season = await query.FirstOrDefaultAsync();

                if (season == null)
                    throw new Exception("La temporada no fue encontrada.");

                season.SeasonName = model.SeasonName;
                season.StartDate = model.StartDate;
                season.EndDate = model.EndDate;
                season.PriceMultiplier = model.PriceMultiplier;

                var result = await _repository.Update(season);
                if (!result)
                    throw new Exception("No se pudo actualizar la temporada.");

                return _mapper.Map<SeasonDTO>(season);
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> InactivateAsync(int id)
        {
            try
            {
                var query = _repository.Query(r => r.IdSeason == id && r.DeletedAt == null);
                var season = await query.FirstOrDefaultAsync();

                if (season == null)
                    throw new Exception("La temporada no fue encontrada.");

                season.DeletedAt = DateTime.UtcNow;

                var result = await _repository.Update(season);
                if (!result)
                    throw new Exception("No se pudo inactivar la temporada.");

                return result;
            }
            catch
            {
                throw;
            }
        }
    }
}
