using CarRental.Automotor.API.DataTransferObjects;
using CarRental.Automotor.API.Mappers;
using CarRental.Automotor.Application.IService;
using Microsoft.AspNetCore.Mvc;

namespace CarRental.Automotor.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClientController(IClientService service) : ControllerBase
    {
        private readonly IClientService _service = service;

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateClientDTO dto)
        {
            var entity = ClientMapper.ToEntity(dto);
            var result = await _service.CreateAsync(entity);

            var resultDto = ClientMapper.ToDto(result);

            return CreatedAtAction(nameof(GetById), new { id = resultDto.Id }, resultDto);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);

            if (result is null)
                return NotFound();

            return Ok(ClientMapper.ToDto(result));
        }

        [HttpPut("/{id:int}")]
        public async Task<IActionResult> Update(int id, UpdateClientDTO dto)
        {
            var entity = ClientMapper.ToEntity(dto, id);
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
