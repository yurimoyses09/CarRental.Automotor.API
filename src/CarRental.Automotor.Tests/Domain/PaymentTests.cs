using CarRental.Automotor.Domain.Entities;
using CarRental.Automotor.Domain.Enums;
using Xunit;

namespace CarRental.Automotor.Tests.Domain
{
    public class PaymentTests
    {
        [Fact]
        public void NewPayment_StartsAsPendingWithoutPaidDate()
        {
            var payment = new Payment(1, 250m);

            Assert.Equal(1, payment.RentalId);
            Assert.Equal(250m, payment.Amount);
            Assert.Equal(PaymentStatus.Pending, payment.Status);
            Assert.Null(payment.PaidAt);
        }

        [Fact]
        public void Approve_SetsApprovedStatusAndPaidDate()
        {
            var payment = new Payment(1, 250m);

            payment.Approve();

            Assert.Equal(PaymentStatus.Approved, payment.Status);
            Assert.NotNull(payment.PaidAt);
        }
    }
}
