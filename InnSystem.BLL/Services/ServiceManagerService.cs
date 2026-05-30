using AutoMapper;
using InnSystem.BLL.Services.Contract;
using InnSystem.DAL.Repositories.Contract;
using InnSystem.DTO.Rooms;
using InnSystem.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InnSystem.BLL.Services
{
    public class ServiceManagerService : IServiceManagerService
    {
        private readonly IGenericRepository<Service> _repository;
        private readonly IMapper _mapper;

        public ServiceManagerService(IGenericRepository<Service> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<ServiceDTO>> GetAllAsync()
        {
            try
            {
                var query = _repository.Query(r => r.DeletedAt == null);
                return _mapper.Map<List<ServiceDTO>>(await query.ToListAsync());
            }
            catch
            {
                throw;
            }
        }

        public async Task<ServiceDTO> GetByIdAsync(int id)
        {
            try
            {
                var query = _repository.Query(r => r.IdService == id && r.DeletedAt == null);
                var service = await query.FirstOrDefaultAsync();
                
                if (service == null)
                    throw new Exception("El servicio no fue encontrado.");

                return _mapper.Map<ServiceDTO>(service);
            }
            catch
            {
                throw;
            }
        }

        public async Task<ServiceDTO> CreateAsync(ServiceCreateDTO model)
        {
            try
            {
                var dbModel = _mapper.Map<Service>(model);
                var created = await _repository.Create(dbModel);
                if (created.IdService == 0)
                    throw new Exception("No se pudo crear el servicio.");

                return _mapper.Map<ServiceDTO>(created);
            }
            catch
            {
                throw;
            }
        }

        public async Task<ServiceDTO> UpdateAsync(int id, ServiceUpdateDTO model)
        {
            try
            {
                var query = _repository.Query(r => r.IdService == id && r.DeletedAt == null);
                var service = await query.FirstOrDefaultAsync();

                if (service == null)
                    throw new Exception("El servicio no fue encontrado.");

                service.Name = model.Name;

                var result = await _repository.Update(service);
                if (!result)
                    throw new Exception("No se pudo actualizar el servicio.");

                return _mapper.Map<ServiceDTO>(service);
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
                var query = _repository.Query(r => r.IdService == id && r.DeletedAt == null);
                var service = await query.FirstOrDefaultAsync();

                if (service == null)
                    throw new Exception("El servicio no fue encontrado.");

                service.DeletedAt = DateTime.UtcNow;

                var result = await _repository.Update(service);
                if (!result)
                    throw new Exception("No se pudo inactivar el servicio.");

                return result;
            }
            catch
            {
                throw;
            }
        }
    }
}
