using InnSystem.API.Utility;
using InnSystem.BLL.Services;
using InnSystem.BLL.Services.Contract;
using InnSystem.DTO.Common;
using InnSystem.DTO.Rooms;
using InnSystem.DTO.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace InnSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }


        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var rsp = new Response<List<UserDTO>>();
            try
            {
                rsp.status = true;
                rsp.value = await _userService.GetAllAsync();

            }
            catch (Exception ex)
            {
                rsp.status = false;
                rsp.msg = ex.Message;
            }
            return Ok(rsp);
        }

        [HttpGet("paginated")]
        public async Task<IActionResult> GetPaginated([FromQuery] string? search, [FromQuery] string? roleName, [FromQuery] int page = 1, [FromQuery] int limit = 8)
        {
            var rsp = new Response<PagedResultDTO<UserDTO>>();
            try
            {
                rsp.status = true;
                rsp.value = await _userService.GetPaginatedAsync(search, roleName, page, limit);
            }
            catch (Exception ex)
            {
                rsp.status = false;
                rsp.msg = ex.Message;
            }
            return Ok(rsp);
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO model)
        {
            var rsp = new Response<SessionDTO>();
            try
            {
                var session = await _userService.Login(model.Email, model.Password);
                rsp.status = true;
                rsp.value = session;
                return Ok(rsp);
            }
            catch (UnauthorizedAccessException ex)
            {
                rsp.status = false;
                rsp.msg = ex.Message;
                return Unauthorized(rsp);
            }
            catch (Exception ex)
            {
                rsp.status = false;
                rsp.msg = ex.Message;
                return StatusCode(StatusCodes.Status500InternalServerError, rsp);
            }
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateAsync([FromBody] UserCreateDTO model)
        {
            var rsp = new Response<UserDTO>();
            try
            {
                var user = await _userService.Create(model);
                rsp.status = true;
                rsp.value = user;
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
                rsp.msg = ex.Message + (ex.InnerException != null ? " | " + ex.InnerException.Message : "");
                return StatusCode(StatusCodes.Status500InternalServerError, rsp);
            }
        }
        [HttpPut("Update/{idUser}")]
        public async Task<IActionResult> UpdateUser(Guid idUser, [FromBody] UserUpdateDTO model)
        {
            var rsp = new Response<bool>();
            try
            {
                rsp.status = true;
                rsp.value = await _userService.UpdateUser(idUser, model);
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

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDTO model)
        {
            var rsp = new Response<bool>();
            try
            {
                rsp.status = true;
                rsp.value = await _userService.ForgotPasswordAsync(model.Email);
                return Ok(rsp);
            }
            catch (Exception ex)
            {
                rsp.status = false;
                rsp.msg = "Ocurrió un error al solicitar la recuperación.";
                return StatusCode(StatusCodes.Status500InternalServerError, rsp);
            }
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDTO model)
        {
            var rsp = new Response<bool>();
            try
            {
                rsp.status = true;
                rsp.value = await _userService.ResetPasswordAsync(model);
                return Ok(rsp);
            }
            catch (UnauthorizedAccessException ex)
            {
                rsp.status = false;
                rsp.msg = ex.Message;
                return BadRequest(rsp);
            }
            catch (Exception ex)
            {
                rsp.status = false;
                rsp.msg = "Ocurrió un error al restablecer la contraseña.";
                return StatusCode(StatusCodes.Status500InternalServerError, rsp);
            }
        }
    }
}
