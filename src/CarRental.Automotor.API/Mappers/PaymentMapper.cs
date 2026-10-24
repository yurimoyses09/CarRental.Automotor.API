using CarRental.Automotor.API.DataTransferObjects;
using CarRental.Automotor.Domain.Entities;

namespace CarRental.Automotor.API.Mappers
{
    public static class PaymentMapper
    {
        public static Payment ToEntity(CreatePaymentDTO dto) =>
            new(dto.RentalId, dto.Amount);

        public static GetPaymentDTO ToDto(Payment payment) =>
            new()
            {
                Id = payment.Id,
                RentalId = payment.RentalId,
                Amount = payment.Amount,
                Status = payment.Status,
                PaidAt = payment.PaidAt,
            };
    }
}
