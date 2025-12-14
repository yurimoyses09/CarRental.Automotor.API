using Locadora.AutoMotors.API.DataTransferObjects;
using Locadora.AutoMotors.Domain.Entities;

namespace Locadora.AutoMotors.API.Mappers
{
    public static class ClienteMapper
    {
        public static Cliente ToEntity(CreateClienteDTO dto) => 
            new Cliente(dto.Nome, dto.Idade, dto.Documento);

        public static GetClienteDTO ToGetDto(Cliente cliente) =>
            new GetClienteDTO
            {
                Id = cliente.Id,
                Documento = cliente.Documento,
                Idade = cliente.Idade,
                Nome = cliente.Nome,
            };
    }
}
