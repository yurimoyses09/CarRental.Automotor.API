using Locadora.AutoMotors.API.DataTransferObjects;
using Locadora.AutoMotors.API.Mappers;
using Locadora.AutoMotors.Application.IService;
using Microsoft.AspNetCore.Mvc;

namespace Locadora.AutoMotors.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AutomovelController : ControllerBase
    {
        private readonly ILogger<AutomovelController> _logger;
        private readonly IAutomovelService _service;

        public AutomovelController(ILogger<AutomovelController> logger, IAutomovelService service)
        {
            _logger = logger;
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateAutomovelDTO dto)
        {
            _logger.LogInformation($"Criando registro do automovel {dto.Placa}");
            var result = await _service.CreateAsync(AutomovelMapper.ToEntity(dto));

            _logger.LogInformation($"Automovel criado com sucesso {dto.Placa}");
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpGet("/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            _logger.LogInformation($"Obtendo dados do automovel com id {id}");

            var result = await _service.GetByIdAsync(id);

            if (result is null) 
            {
                _logger.LogInformation($"Automovel com id {id} não existe no sistema.");
                return NotFound();
            }

            return Ok(AutomovelMapper.ToGetDto(result));
        }
    }
}
