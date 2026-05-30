using InnSystem.BLL.Services.Contract;
using InnSystem.DTO.Catalogs;
using InnSystem.API.Utility;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InnSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomTypeController : ControllerBase
    {
        private readonly IRoomTypeService _roomTypeService;

        public RoomTypeController(IRoomTypeService roomTypeService)
        {
            _roomTypeService = roomTypeService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = new Response<List<RoomTypeDTO>>();
            try
            {
                response.status = true;
                response.value = await _roomTypeService.GetAllAsync();
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
            var response = new Response<RoomTypeDTO>();
            try
            {
                response.status = true;
                response.value = await _roomTypeService.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                response.status = false;
                response.msg = ex.Message;
            }
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] RoomTypeCreateDTO model)
        {
            var response = new Response<RoomTypeDTO>();
            try
            {
                response.status = true;
                response.value = await _roomTypeService.CreateAsync(model);
            }
            catch (Exception ex)
            {
                response.status = false;
                response.msg = ex.Message;
            }
            return Ok(response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] RoomTypeUpdateDTO model)
        {
            var response = new Response<RoomTypeDTO>();
            try
            {
                if (id != model.IdRoomType)
                {
                    response.status = false;
                    response.msg = "El ID de la ruta no coincide con el del modelo.";
                    return BadRequest(response);
                }

                response.status = true;
                response.value = await _roomTypeService.UpdateAsync(id, model);
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
                response.value = await _roomTypeService.InactivateAsync(id);
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
