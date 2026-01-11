using CarRental.Automotor.API.DataTransferObjects;
using CarRental.Automotor.API.Mappers;
using CarRental.Automotor.Application.IService;
using Microsoft.AspNetCore.Mvc;

namespace CarRental.Automotor.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AutomobileController(IAutomobileService service) : ControllerBase
    {
        private readonly IAutomobileService _service = service;

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateAutomobileDTO dto)
        {
            var entity = AutomobileMapper.ToEntity(dto);
            var result = await _service.CreateAsync(entity);

            var resultDto = AutomobileMapper.ToDto(result);

            return CreatedAtAction(nameof(GetById), new { id = resultDto.Id }, resultDto);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<GetAutomobileDTO>> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);

            if (result is null) 
                return NotFound();

            return Ok(AutomobileMapper.ToDto(result));
        }

        [HttpPut("/{id:int}")]
        public async Task<IActionResult> Update(int id, UpdateAutomobileDTO dto)
        {
            var entity = AutomobileMapper.ToEntity(dto, id);
            var updated = await _service.UpdateAsync(entity);

            if (updated == 0) return NotFound();

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteById(int id)
        {
            var result = await _service.DeleteByIdAsync(id);
            if (result == 0) return NotFound();
            return NoContent();
        }
    }
}
