using CarRental.Automotor.API.DataTransferObjects;
using CarRental.Automotor.Domain.Entities;

namespace CarRental.Automotor.API.Mappers
{
    public static class ClientMapper
    {
        public static Client ToEntity(CreateClientDTO dto) => 
            new(dto.Name, dto.Age, dto.Document);


        public static Client ToEntity(UpdateClientDTO dto, int id) =>
            new(dto.Name, dto.Age, dto.Document, id);


        public static GetClientDTO ToDto(Client cliente) =>
            new()
            {
                Id = cliente.Id,
                Document = cliente.Document,
                Age = cliente.Age,
                Name = cliente.Name,
            };
    }
}
