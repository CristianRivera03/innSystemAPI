using InnSystem.DTO.Catalogs;
using System;
using System.Collections.Generic;
using System.Text;

namespace InnSystem.BLL.Services.Contract
{
    public interface ICatalogService
    {
        Task<CatalogDTO> GetAllCatalogsAsync();
    }
}
