using InnSystem.DTO.Roles;
using System;
using System.Collections.Generic;
using System.Text;

namespace InnSystem.BLL.Services.Contract
{
    public interface IRoleService
    {
        Task<List<RoleDTO>> GetAllAsync();

        Task<List<int>> GetAssignedModuleIds(int roleId);

        Task<bool> UpdateRolePermissions(AssignModulesDTO request);

        Task<RoleDTO> CreateRole(RoleCreateDTO request);
    }
}
