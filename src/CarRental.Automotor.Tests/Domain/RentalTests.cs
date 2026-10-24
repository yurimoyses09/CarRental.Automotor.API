using CarRental.Automotor.Domain.Entities;
using CarRental.Automotor.Domain.Exceptions;
using Xunit;

namespace CarRental.Automotor.Tests.Domain
{
    public class RentalTests
    {
        private static readonly DateTimeOffset Start = new(2026, 10, 1, 10, 0, 0, TimeSpan.Zero);

        [Fact]
        public void CalculateTotalAmount_MultipliesDailyRateByNumberOfDays()
        {
            var total = Rental.CalculateTotalAmount(100m, Start, Start.AddDays(3));

            Assert.Equal(300m, total);
        }

        [Fact]
        public void CalculateTotalAmount_RoundsPartialDayUp()
        {
            var total = Rental.CalculateTotalAmount(100m, Start, Start.AddDays(2).AddHours(5));

            Assert.Equal(300m, total);
        }

        [Fact]
        public void CalculateTotalAmount_SameDayRentalCountsAsOneDay()
        {
            var total = Rental.CalculateTotalAmount(100m, Start, Start);

            Assert.Equal(100m, total);
        }

        [Fact]
        public void CalculateTotalAmount_WhenEndIsBeforeStart_ThrowsBusinessException()
        {
            Assert.Throws<BusinessException>(() => Rental.CalculateTotalAmount(100m, Start, Start.AddDays(-1)));
        }
    }
}
