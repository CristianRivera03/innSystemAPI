using InnSystem.DTO.Roles;
using System;
using System.Collections.Generic;
using System.Text;

namespace InnSystem.BLL.Services.Contract
{
    public interface IRoleService
    {
        Task<List<RoleDTO>> GetAllAsync();
    }
}
