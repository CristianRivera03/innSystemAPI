using InnSystem.BLL.Services.Contract;
using InnSystem.DTO.Rooms;
using InnSystem.API.Utility;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InnSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceController : ControllerBase
    {
        private readonly IServiceManagerService _serviceManager;

        public ServiceController(IServiceManagerService serviceManager)
        {
            _serviceManager = serviceManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = new Response<List<ServiceDTO>>();
            try
            {
                response.status = true;
                response.value = await _serviceManager.GetAllAsync();
            }
            catch (Exception ex)
            {
                response.status = false;
                response.msg = ex.Message;
            }
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var response = new Response<ServiceDTO>();
            try
            {
                response.status = true;
                response.value = await _serviceManager.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                response.status = false;
                response.msg = ex.Message;
            }
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ServiceCreateDTO model)
        {
            var response = new Response<ServiceDTO>();
            try
            {
                response.status = true;
                response.value = await _serviceManager.CreateAsync(model);
            }
            catch (Exception ex)
            {
                response.status = false;
                response.msg = ex.Message;
            }
            return Ok(response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ServiceUpdateDTO model)
        {
            var response = new Response<ServiceDTO>();
            try
            {
                if (id != model.IdService)
                {
                    response.status = false;
                    response.msg = "El ID de la ruta no coincide con el del modelo.";
                    return BadRequest(response);
                }

                response.status = true;
                response.value = await _serviceManager.UpdateAsync(id, model);
            }
            catch (Exception ex)
            {
                response.status = false;
                response.msg = ex.Message;
            }
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Inactivate(int id)
        {
            var response = new Response<bool>();
            try
            {
                response.status = true;
                response.value = await _serviceManager.InactivateAsync(id);
            }
            catch (Exception ex)
            {
                response.status = false;
                response.msg = ex.Message;
            }
            return Ok(response);
        }
    }
}
