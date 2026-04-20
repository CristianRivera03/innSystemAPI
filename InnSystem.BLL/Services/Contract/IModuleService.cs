using InnSystem.DTO.Roles;
using System;
using System.Collections.Generic;
using System.Text;

namespace InnSystem.BLL.Services.Contract
{
    public interface IModuleService
    {
        Task<List<ModuleDTO>> GetAllAsync();

    }
}
