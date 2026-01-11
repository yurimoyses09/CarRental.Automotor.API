using CarRental.Automotor.API.DataTransferObjects;
using CarRental.Automotor.Domain.Entities;

namespace CarRental.Automotor.API.Mappers
{
    public static class AutomobileMapper
    {
        public static Automobile ToEntity(CreateAutomobileDTO dto)
            => new(dto.Model, dto.YearManufacture, dto.Plate, dto.FuelType, dto.Color);

        public static GetAutomobileDTO ToDto(Automobile Automobile)
        {
            return new GetAutomobileDTO()
            {
                YearManufacture = Automobile.YearManufacture,
                FuelType = Automobile.FuelType,
                Color = Automobile.Color,
                Id = Automobile.Id,
                Model = Automobile.Model,
                Plate = Automobile.Plate
            };
        }

        public static Automobile ToEntity(UpdateAutomobileDTO dto, int id)
            => new(dto.Model, dto.YearManufacture, dto.Plate, dto.FuelType, dto.Color, id);
    }
}
