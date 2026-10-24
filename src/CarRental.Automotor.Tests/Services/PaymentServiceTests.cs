using CarRental.Automotor.Application.Events;
using CarRental.Automotor.Application.IRepository;
using CarRental.Automotor.Application.IService;
using CarRental.Automotor.Application.Service;
using CarRental.Automotor.Domain.Entities;
using CarRental.Automotor.Domain.Enums;
using CarRental.Automotor.Domain.Exceptions;
using Moq;
using Xunit;

namespace CarRental.Automotor.Tests.Services
{
    public class PaymentServiceTests
    {
        private readonly Mock<IPaymentRepository> _paymentRepository = new();
        private readonly Mock<IRentalRepository> _rentalRepository = new();
        private readonly Mock<IPaymentEventPublisher> _eventPublisher = new();
        private readonly PaymentService _service;

        public PaymentServiceTests()
        {
            _service = new PaymentService(
                _paymentRepository.Object,
                _rentalRepository.Object,
                _eventPublisher.Object);
        }

        private static Rental CreateRental(int id) =>
            new(1, 1, DateTimeOffset.UtcNow, DateTimeOffset.UtcNow.AddDays(3), null, 100m, 300m, RentalStatus.Active, id);

        [Fact]
        public async Task CreateAsync_WhenRentalExists_SavesApprovedPaymentAndPublishesEvent()
        {
            _rentalRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(CreateRental(1));
            _paymentRepository.Setup(r => r.CreateAsync(It.IsAny<Payment>()))
                .ReturnsAsync((Payment p) => p);

            var result = await _service.CreateAsync(new Payment(1, 300m));

            Assert.Equal(PaymentStatus.Approved, result.Status);
            Assert.NotNull(result.PaidAt);

            _paymentRepository.Verify(r => r.CreateAsync(It.IsAny<Payment>()), Times.Once);
            _eventPublisher.Verify(p => p.PublishPaymentProcessedAsync(
                It.Is<PaymentProcessedEvent>(e =>
                    e.RentalId == 1 &&
                    e.Amount == 300m &&
                    e.Status == PaymentStatus.Approved)), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_WhenRentalDoesNotExist_ThrowsKeyNotFoundException()
        {
            _rentalRepository.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Rental?)null);

            await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.CreateAsync(new Payment(99, 100m)));

            _paymentRepository.Verify(r => r.CreateAsync(It.IsAny<Payment>()), Times.Never);
            _eventPublisher.Verify(p => p.PublishPaymentProcessedAsync(It.IsAny<PaymentProcessedEvent>()), Times.Never);
        }

        [Fact]
        public async Task CreateAsync_WhenAmountDiffersFromRentalTotal_ThrowsBusinessExceptionAndDoesNotSave()
        {
            _rentalRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(CreateRental(1));

            await Assert.ThrowsAsync<BusinessException>(() => _service.CreateAsync(new Payment(1, 250m)));

            _paymentRepository.Verify(r => r.CreateAsync(It.IsAny<Payment>()), Times.Never);
            _eventPublisher.Verify(p => p.PublishPaymentProcessedAsync(It.IsAny<PaymentProcessedEvent>()), Times.Never);
        }

        [Fact]
        public async Task CreateAsync_WhenAmountIsNotPositive_ThrowsArgumentException()
        {
            await Assert.ThrowsAsync<ArgumentException>(() => _service.CreateAsync(new Payment(1, 0m)));

            _paymentRepository.Verify(r => r.CreateAsync(It.IsAny<Payment>()), Times.Never);
            _eventPublisher.Verify(p => p.PublishPaymentProcessedAsync(It.IsAny<PaymentProcessedEvent>()), Times.Never);
        }

        [Fact]
        public async Task GetByIdAsync_WhenPaymentExists_ReturnsPayment()
        {
            var payment = new Payment(1, 300m) { Id = 5 };
            _paymentRepository.Setup(r => r.GetByIdAsync(5)).ReturnsAsync(payment);

            var result = await _service.GetByIdAsync(5);

            Assert.Same(payment, result);
        }
    }
}
