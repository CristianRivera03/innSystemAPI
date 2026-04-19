using InnSystem.API.Utility;
using InnSystem.BLL.Services;
using InnSystem.BLL.Services.Contract;
using InnSystem.DTO.Bookings;
using InnSystem.DTO.Rooms;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InnSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomController : ControllerBase
    {

        private readonly IRoomService _roomService;

        public RoomController(IRoomService roomService)
        {
            _roomService = roomService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var rsp = new Response<List<RoomDTO>>();
            try
            {
                rsp.status = true;
                rsp.value = await _roomService.GetAllAsync();

            }
            catch (Exception ex)
            {
                rsp.status = false;
                rsp.msg = ex.Message;
            }
            return Ok(rsp);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsyncById(int id)
        {
            var rsp = new Response<RoomDTO>();
            try
            {
                var room = await _roomService.GetByIdAsync(id);

                if(room == null)
                {
                    rsp.status = false;
                    rsp.msg = "Habitacion no encontrada";
                    return NotFound(rsp);
                }

                rsp.status = true;
                rsp.value = room;
                return Ok(rsp);

            }
            catch (Exception ex)
            {
                rsp.status = false;
                rsp.msg = ex.Message;
                return StatusCode(StatusCodes.Status500InternalServerError, rsp); 
            }
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] RoomUpdateDTO model)
        {
            var rsp = new Response<bool>();

            try
            {
                var success = await _roomService.UpdateAsync(id, model);

                if (!success)
                {
                    rsp.status = false;
                    rsp.msg = "Error al actualizar habitacion";
                    return NotFound(rsp);
                }

                rsp.status = true;
                rsp.value = true;
                return Ok(rsp);
            }
            catch (Exception ex)
            {
                rsp.status = false;
                rsp.msg = ex.Message;
                return StatusCode(500, rsp);
            }
        }


        [HttpGet("available")]
        public async Task<IActionResult> GetAvailableRooms([FromQuery] DateTime checkIn, [FromQuery] DateTime checkOut, [FromQuery] int guestsCount)
        {
            var rsp = new Response<List<RoomDTO>>();
            try
            {
                var availableRooms = await _roomService.GetAvailableRoomsAsync(checkIn, checkOut, guestsCount);
                rsp.status = true;
                rsp.value = availableRooms;
                return Ok(rsp);
            }
            catch (ArgumentException ex)
            {
                rsp.status = false;
                rsp.msg = ex.Message;
                return BadRequest(rsp);
            }
            catch (Exception ex)
            {
                rsp.status = false;
                rsp.msg = ex.Message;
                return StatusCode(StatusCodes.Status500InternalServerError, rsp);
            }
        }
        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] RoomCreateDTO model)
        {
            var rsp = new Response<RoomDTO>();
            try
            {
                var room = await _roomService.CreateAsync(model);
                rsp.status = true;
                rsp.value = room;
                return Ok(rsp);
            }
            catch (TaskCanceledException ex)
            {
                rsp.status = false;
                rsp.msg = ex.Message;
                return BadRequest(rsp);
            }
            catch (Exception ex)
            {
                rsp.status = false;
                rsp.msg = ex.Message;
                return StatusCode(StatusCodes.Status500InternalServerError, rsp);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> SoftDeleteAsync(int id)
        {
            var rsp = new Response<bool>();
            try
            {
                var success = await _roomService.SoftDelete(id);
                rsp.status = success;
                rsp.value = success;
                if (!success)
                {
                    rsp.msg = "Habitacion no encontrada o no se pudo eliminar.";
                    return NotFound(rsp);
                }
                return Ok(rsp);
            }
            catch (TaskCanceledException ex)
            {
                rsp.status = false;
                rsp.msg = ex.Message;
                return BadRequest(rsp);
            }
            catch (Exception ex)
            {
                rsp.status = false;
                rsp.msg = ex.Message;
                return StatusCode(StatusCodes.Status500InternalServerError, rsp);
            }
        }

        [HttpDelete("{id}/hard")]
        public async Task<IActionResult> HardDeleteAsync(int id)
        {
            var rsp = new Response<bool>();
            try
            {
                var success = await _roomService.HardDelete(id);
                rsp.status = success;
                rsp.value = success;
                if (!success)
                {
                    rsp.msg = "Habitacion no encontrada.";
                    return NotFound(rsp);
                }
                return Ok(rsp);
            }
            catch (Exception ex)
            {
                rsp.status = false;
                rsp.msg = ex.Message;
                return StatusCode(StatusCodes.Status500InternalServerError, rsp);
            }
        }

        [HttpPatch("{id}/status")]
        public async Task<IActionResult> ChangeOperationalStatusAsync(int id, [FromBody] int statusId)
        {
            var rsp = new Response<bool>();
            try
            {
                var success = await _roomService.ChangeOperationalStatusAsync(id, statusId);
                rsp.status = success;
                rsp.value = success;
                if (!success)
                {
                    rsp.msg = "Habitacion no encontrada.";
                    return NotFound(rsp);
                }
                return Ok(rsp);
            }
            catch (Exception ex)
            {
                rsp.status = false;
                rsp.msg = ex.Message;
                return StatusCode(StatusCodes.Status500InternalServerError, rsp);
            }
        }
    }
}
