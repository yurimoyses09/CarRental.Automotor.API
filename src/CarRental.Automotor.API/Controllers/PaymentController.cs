using CarRental.Automotor.API.DataTransferObjects;
using CarRental.Automotor.API.Mappers;
using CarRental.Automotor.Application.IService;
using Microsoft.AspNetCore.Mvc;

namespace CarRental.Automotor.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PaymentController(IPaymentService service) : ControllerBase
    {
        private readonly IPaymentService _service = service;

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePaymentDTO dto)
        {
            var entity = PaymentMapper.ToEntity(dto);
            var result = await _service.CreateAsync(entity);

            var resultDto = PaymentMapper.ToDto(result);

            return CreatedAtAction(nameof(GetById), new { id = resultDto.Id }, resultDto);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);

            if (result is null)
                return NotFound();

            return Ok(PaymentMapper.ToDto(result));
        }
    }
}
