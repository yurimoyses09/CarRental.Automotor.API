using CarRental.Automotor.API.DataTransferObjects;
using CarRental.Automotor.Domain.Entities;
using CarRental.Automotor.Domain.Enums;

namespace CarRental.Automotor.API.Mappers
{
    public static class RentalMapper
    {
        public static Rental ToEntity(CreateRentalDTO dto)
            => new(
                dto.ClientId,
                dto.AutomobileId,
                dto.StartDate,
                dto.ExpectedEndDate,
                null,
                dto.DailyRate,
                dto.TotalAmount,
                RentalStatus.Pending
            );


        public static Rental ToEntity(UpdateRentalDTO dto, int id)
            => new(
                dto.ClientId,
                dto.AutomobileId,
                dto.StartDate,
                dto.ExpectedEndDate,
                null,
                dto.DailyRate,
                dto.TotalAmount,
                dto.Status,
                id
            );

        public static GetRentalDTO ToDto(Rental result)
            =>  new()
            {
                Id = result.Id,
                ClientId = result.ClientId,
                AutomobileId = result.AutomobileId,
                StartDate = result.StartDate,
                ExpectedEndDate = result.ExpectedEndDate,
                DailyRate = result.DailyRate,
                TotalAmount = result.TotalAmount,
                ActualEndDate = result.ActualEndDate,
                Status = result.Status,
            };
    }
}
