using AutoMapper;
using InnSystem.BLL.Services.Contract;
using InnSystem.DAL.Repositories.Contract;
using InnSystem.DTO.Roles;
using InnSystem.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace InnSystem.BLL.Services
{
    public class ModuleService : IModuleService
    {
        private readonly IGenericRepository<Module> _moduleRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<IModuleService> _logger;

        public ModuleService(IGenericRepository<Module> moduleRepository, IMapper mapper, ILogger<IModuleService> logger)
        {
            _moduleRepository = moduleRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<List<ModuleDTO>> GetAllAsync()
        {
            try
            {
                var listModules = await _moduleRepository.Query().ToListAsync();

                return _mapper.Map<List<ModuleDTO>>(listModules);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting modules");
                throw;
            }
        }
    }
}
