using InnSystem.API.Utility;
using InnSystem.BLL.Services.Contract;
using InnSystem.DTO.Roles;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InnSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }



        [HttpGet]
        [Route("Get")]
        public async Task<IActionResult> Get()
        {
            var rsp = new Response<List<RoleDTO>>();

            try
            {
                var roles = await _roleService.GetAllAsync();

                if (roles == null || !roles.Any())
                {
                    rsp.status = false;
                    rsp.msg = "No se encontraron roles registrados.";
                    return NotFound(rsp); // Devuelve 404 
                }

                rsp.status = true;
                rsp.value = roles;
                return Ok(rsp);
            }
            catch (Exception ex)
            {
                rsp.status = false;
                rsp.msg = "Error interno al obtener los roles.";


                return StatusCode(500, rsp); // Devuelve 500 si el servidor falla
            }
        }

    }
}
