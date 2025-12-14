using Locadora.AutoMotors.API.DataTransferObjects;
using Locadora.AutoMotors.API.Mappers;
using Locadora.AutoMotors.Application.IService;
using Microsoft.AspNetCore.Mvc;

namespace Locadora.AutoMotors.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClienteController : ControllerBase
    {
        private readonly ILogger<ClienteController> _logger;
        private readonly IClienteService _service;

        public ClienteController(ILogger<ClienteController> logger, IClienteService service)
        {
            _logger = logger;
            _service = service;
        }


        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateClienteDTO dto)
        {
            _logger.LogInformation($"Criando usuario {dto.Nome} no sistema");

            var result = await _service.CreateAsync(ClienteMapper.ToEntity(dto));

            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpGet("/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            _logger.LogInformation($"Buscando cliente com Id {id}");

            var result = await _service.GetByIdAsync(id);

            if (result is null)
                return NotFound();

            return Ok(ClienteMapper.ToGetDto(result));
        }
    }
}
