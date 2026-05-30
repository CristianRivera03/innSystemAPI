using InnSystem.API.Utility;
using InnSystem.BLL.Services.Contract;
using InnSystem.DTO.Catalogs;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InnSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeasonController : ControllerBase
    {
        private readonly ISeasonService _seasonService;

        public SeasonController(ISeasonService seasonService)
        {
            _seasonService = seasonService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = new Response<List<SeasonDTO>>();
            try
            {
                response.status = true;
                response.value = await _seasonService.GetAllAsync();
            }
            catch (Exception ex)
            {
                response.status = false;
                response.msg = ex.Message;
            }
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] SeasonCreateDTO model)
        {
            var response = new Response<SeasonDTO>();
            try
            {
                response.status = true;
                response.value = await _seasonService.CreateAsync(model);
            }
            catch (Exception ex)
            {
                response.status = false;
                response.msg = ex.Message;
            }
            return Ok(response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] SeasonUpdateDTO model)
        {
            var response = new Response<SeasonDTO>();
            try
            {
                response.status = true;
                response.value = await _seasonService.UpdateAsync(id, model);
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
                response.value = await _seasonService.InactivateAsync(id);
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
