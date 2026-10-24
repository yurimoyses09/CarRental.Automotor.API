using CarRental.Automotor.API.DataTransferObjects;
using CarRental.Automotor.API.Mappers;
using CarRental.Automotor.Domain.Entities;
using CarRental.Automotor.Domain.Enums;
using Xunit;

namespace CarRental.Automotor.Tests.Mappers
{
    public class PaymentMapperTests
    {
        [Fact]
        public void ToEntity_MapsRentalIdAndAmount()
        {
            var dto = new CreatePaymentDTO { RentalId = 2, Amount = 450m };

            var entity = PaymentMapper.ToEntity(dto);

            Assert.Equal(2, entity.RentalId);
            Assert.Equal(450m, entity.Amount);
            Assert.Equal(PaymentStatus.Pending, entity.Status);
        }

        [Fact]
        public void ToDto_MapsAllFields()
        {
            var paidAt = DateTimeOffset.UtcNow;
            var payment = new Payment(2, 450m) { Id = 7 };
            payment.Approve();

            var dto = PaymentMapper.ToDto(payment);

            Assert.Equal(7, dto.Id);
            Assert.Equal(2, dto.RentalId);
            Assert.Equal(450m, dto.Amount);
            Assert.Equal(PaymentStatus.Approved, dto.Status);
            Assert.NotNull(dto.PaidAt);
            Assert.True(dto.PaidAt >= paidAt);
        }
    }
}
