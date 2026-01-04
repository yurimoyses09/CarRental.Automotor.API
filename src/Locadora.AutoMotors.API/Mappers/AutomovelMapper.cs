using Locadora.AutoMotors.API.DataTransferObjects;
using Locadora.AutoMotors.Domain.Entities;

namespace Locadora.AutoMotors.API.Mappers
{
    public static class AutomovelMapper
    {
        public static Automovel ToEntity(CreateAutomovelDTO dto)
            => new(dto.Modelo, dto.AnoFabricacao, dto.Placa, dto.Combustivel, dto.Cor);

        public static GetAutomovelDTO ToGetDto(Automovel automovel)
        {
            return new GetAutomovelDTO()
            {
                AnoFabricacao = automovel.AnoFabricacao,
                Combustivel = automovel.Combustivel,
                Cor = automovel.Cor,
                Id = automovel.Id,
                Modelo = automovel.Modelo,
                Placa = automovel.Placa
            };
        }
    }
}
