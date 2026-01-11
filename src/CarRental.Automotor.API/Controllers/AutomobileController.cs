using CarRental.Automotor.API.DataTransferObjects;
using CarRental.Automotor.API.Mappers;
using CarRental.Automotor.Application.IService;
using Microsoft.AspNetCore.Mvc;

namespace CarRental.Automotor.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AutomobileController : ControllerBase
    {
        private readonly ILogger<AutomobileController> _logger;
        private readonly IAutomobileService _service;

        public AutomobileController(ILogger<AutomobileController> logger, IAutomobileService service)
        {
            _logger = logger;
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateAutomobileDTO dto)
        {
            _logger.LogInformation($"Criando registro do Automobile {dto.Plate}");
            var result = await _service.CreateAsync(AutomobileMapper.ToEntity(dto));

            _logger.LogInformation($"Automobile criado com sucesso {dto.Plate}");
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpGet("/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            _logger.LogInformation($"Obtendo dados do Automobile com id {id}");
            var result = await _service.GetByIdAsync(id);

            if (result is null) 
            {
                _logger.LogInformation($"Automobile com id {id} não existe no sistema.");
                return NotFound();
            }

            return Ok(AutomobileMapper.ToDto(result));
        }
    }
}
