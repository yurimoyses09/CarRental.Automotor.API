using CarRental.Automotor.Domain.Entities;
using CarRental.Automotor.Domain.Exceptions;
using Xunit;

namespace CarRental.Automotor.Tests.Domain
{
    // Regra: o valor total do aluguel é diária × dias. Dia parcial conta como dia
    // inteiro e o mínimo cobrado é 1 dia. Devolução antes da retirada é inválida.
    public class RentalCalculateTotalAmountTests
    {
        private static readonly DateTimeOffset Start = new(2026, 10, 1, 9, 0, 0, TimeSpan.Zero);

        [Fact]
        public void CalculateTotalAmount_ForExactDays_MultipliesDailyRate()
        {
            var total = Rental.CalculateTotalAmount(100m, Start, Start.AddDays(3));

            Assert.Equal(300m, total);
        }

        [Fact]
        public void CalculateTotalAmount_ForPartialDay_ChargesWholeDay()
        {
            // 2 dias e 1 hora: o dia extra entra como dia inteiro.
            var total = Rental.CalculateTotalAmount(100m, Start, Start.AddDays(2).AddHours(1));

            Assert.Equal(300m, total);
        }

        [Fact]
        public void CalculateTotalAmount_ForLessThanOneDay_ChargesMinimumOneDay()
        {
            var total = Rental.CalculateTotalAmount(100m, Start, Start.AddHours(5));

            Assert.Equal(100m, total);
        }

        [Fact]
        public void CalculateTotalAmount_WhenStartAndEndAreEqual_ChargesMinimumOneDay()
        {
            var total = Rental.CalculateTotalAmount(100m, Start, Start);

            Assert.Equal(100m, total);
        }

        [Fact]
        public void CalculateTotalAmount_WhenEndIsBeforeStart_ThrowsBusinessException()
        {
            Assert.Throws<BusinessException>(() =>
                Rental.CalculateTotalAmount(100m, Start, Start.AddDays(-1)));
        }
    }
}
