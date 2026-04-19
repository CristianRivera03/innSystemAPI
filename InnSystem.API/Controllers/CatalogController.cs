using InnSystem.API.Utility;
using InnSystem.BLL.Services.Contract;
using InnSystem.DTO.Catalogs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InnSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CatalogController : ControllerBase
    {
        private readonly ICatalogService _catalogService;

        public CatalogController(ICatalogService catalogService)
        {
            _catalogService = catalogService;
        }

        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var rsp = new Response<CatalogDTO>();

            try
            {
                var catalogs = await _catalogService.GetAllCatalogsAsync();

                if (catalogs == null)
                {
                    rsp.status = false;
                    rsp.msg = "No se encontraron catálogos.";
                    return NotFound(rsp); 
                }

                rsp.status = true;
                rsp.value = catalogs;
                return Ok(rsp);
            }
            catch (Exception ex)
            {
                rsp.status = false;
                rsp.msg = "Error interno al obtener los catálogos.";

                return StatusCode(500, rsp); 
            }
        }
    }
}
