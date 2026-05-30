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
    public class RoomTypeService : IRoomTypeService
    {
        private readonly IGenericRepository<RoomType> _repository;
        private readonly IMapper _mapper;

        public RoomTypeService(IGenericRepository<RoomType> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<RoomTypeDTO>> GetAllAsync()
        {
            try
            {
                var query = _repository.Query(r => r.DeletedAt == null);
                return _mapper.Map<List<RoomTypeDTO>>(await query.ToListAsync());
            }
            catch
            {
                throw;
            }
        }

        public async Task<RoomTypeDTO> GetByIdAsync(int id)
        {
            try
            {
                var query = _repository.Query(r => r.IdRoomType == id && r.DeletedAt == null);
                var roomType = await query.FirstOrDefaultAsync();
                
                if (roomType == null)
                    throw new Exception("El tipo de habitación no fue encontrado.");

                return _mapper.Map<RoomTypeDTO>(roomType);
            }
            catch
            {
                throw;
            }
        }

        public async Task<RoomTypeDTO> CreateAsync(RoomTypeCreateDTO model)
        {
            try
            {
                var dbModel = _mapper.Map<RoomType>(model);
                var created = await _repository.Create(dbModel);
                if (created.IdRoomType == 0)
                    throw new Exception("No se pudo crear el tipo de habitación.");

                return _mapper.Map<RoomTypeDTO>(created);
            }
            catch
            {
                throw;
            }
        }

        public async Task<RoomTypeDTO> UpdateAsync(int id, RoomTypeUpdateDTO model)
        {
            try
            {
                var query = _repository.Query(r => r.IdRoomType == id && r.DeletedAt == null);
                var roomType = await query.FirstOrDefaultAsync();

                if (roomType == null)
                    throw new Exception("El tipo de habitación no fue encontrado.");

                roomType.Name = model.Name;
                roomType.Description = model.Description;
                roomType.GuestCapacity = model.GuestCapacity;

                var result = await _repository.Update(roomType);
                if (!result)
                    throw new Exception("No se pudo actualizar el tipo de habitación.");

                return _mapper.Map<RoomTypeDTO>(roomType);
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
                var query = _repository.Query(r => r.IdRoomType == id && r.DeletedAt == null);
                var roomType = await query.FirstOrDefaultAsync();

                if (roomType == null)
                    throw new Exception("El tipo de habitación no fue encontrado.");

                roomType.DeletedAt = DateTime.UtcNow;

                var result = await _repository.Update(roomType);
                if (!result)
                    throw new Exception("No se pudo inactivar el tipo de habitación.");

                return result;
            }
            catch
            {
                throw;
            }
        }
    }
}
