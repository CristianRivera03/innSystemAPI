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
    public class RoleService : IRoleService
    {
        private readonly IGenericRepository<Role> _roleRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<RoleService> _logger;

        public RoleService(IGenericRepository<Role> roleRepository, IMapper mapper, ILogger<RoleService> logger)
        {
            _roleRepository = roleRepository;
            _mapper = mapper;
            _logger = logger;
        }

        //Para traer lista de roles
        public async Task<List<RoleDTO>> GetAllAsync()
        {
            try
            {

                var query =  _roleRepository.Query();

                var rolesList = await query.ToListAsync(); 

                return _mapper.Map<List<RoleDTO>>(rolesList);

            }catch(Exception ex)
            {
                _logger.LogError(ex, "Error al obtener los roles");
                throw;
            }
        }
    }
}
