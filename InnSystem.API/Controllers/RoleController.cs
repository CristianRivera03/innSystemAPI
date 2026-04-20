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

        [HttpGet]
        [Route("Get/{id}/modules")]

        public async Task<IActionResult> GetAssignedModuleIds(int id)
        {
            var rsp = new Response<List<int>>();

            try
            {

                var listaIDs = await _roleService.GetAssignedModuleIds(id);


                if (listaIDs == null || listaIDs.Count == 0)
                {
                    rsp.status = false;
                    rsp.msg = "No se encontraron registros";
                    rsp.value = new List<int>();
                    return NotFound(rsp);

                }



                rsp.status = true;
                rsp.value = listaIDs;
                return Ok(rsp);
            }
            catch (Exception ex)
            {
                rsp.status = false;
                rsp.msg = "Error en el sistema";
                return StatusCode(500, rsp);
            }
        }


        //Actualizar Permisos
        [HttpPost]
        [Route("UpdatePermissions")]

        public async Task<IActionResult> UpdatePermissions([FromBody] AssignModulesDTO request)
        {
            var rsp = new Response<bool>();

            try
            {
                if (request == null || request.IdRole <= 0)
                {
                    rsp.status = false;
                    rsp.msg = "Datos invalidos";
                    return BadRequest(rsp);
                }

                // se ejectua el metodo
                rsp.status = await _roleService.UpdateRolePermissions(request);
                rsp.value = rsp.status;
                rsp.msg = "Permisos actualizados";

                return Ok(rsp);

            }
            catch (Exception ex)
            {
                rsp.status = false;
                rsp.msg = "Ocurrio un error con el servidor";
                return StatusCode(500, rsp);
            }

        }


    }
}
