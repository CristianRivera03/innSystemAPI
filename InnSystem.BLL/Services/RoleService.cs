using AutoMapper;
using InnSystem.BLL.Services.Contract;
using InnSystem.DAL.Repositories.Contract;
using InnSystem.DTO.Roles;
using InnSystem.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace InnSystem.BLL.Services
{
    public class RoleService : IRoleService
    {
        private readonly IGenericRepository<Role> _roleRepository;
        private readonly IGenericRepository<Module> _moduleRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<RoleService> _logger;

        public RoleService(IGenericRepository<Role> roleRepository, IGenericRepository<Module> moduleRepository, IMapper mapper, ILogger<RoleService> logger)
        {
            _roleRepository = roleRepository;
            _moduleRepository = moduleRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public Task<RoleDTO> CreateRole(RoleCreateDTO request)
        {
            throw new NotImplementedException();
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

        public async Task<List<int>> GetAssignedModuleIds(int roleId)
        {
            try
            {
                var moduleIds = await _roleRepository.Query()
                    .Where(r => r.IdRole == roleId)
                    .SelectMany(r => r.IdModules)
                    .Select(m => m.IdModule)
                    .Distinct()
                    .ToListAsync();

                return moduleIds;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "error al obtener los modulos");
                throw;
            }
        }

        public async Task<bool> UpdateRolePermissions(AssignModulesDTO request)
        {
            try
            {
                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    // Load role with its current modules
                    var role = await _roleRepository.Query()
                        .Include(r => r.IdModules)
                        .FirstOrDefaultAsync(r => r.IdRole == request.IdRole);

                    if (role == null)
                        return false;

                    // Clear existing module assignments
                    role.IdModules.Clear();

                    // Assign new modules if any
                    if (request.ModuleIds != null && request.ModuleIds.Any())
                    {
                        var modules = await _moduleRepository.Query()
                            .Where(m => request.ModuleIds.Contains(m.IdModule))
                            .ToListAsync();

                        foreach (var module in modules)
                        {
                            role.IdModules.Add(module);
                        }
                    }

                    var updated = await _roleRepository.Update(role);
                    if (!updated)
                        throw new Exception($"No se pudo actualizar los permisos para el rol {request.IdRole}");

                    transaction.Complete();
                    return true;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar los permisos para el Rol {RoleId}", request.IdRole);
                throw;
            }
        }
    }
}
