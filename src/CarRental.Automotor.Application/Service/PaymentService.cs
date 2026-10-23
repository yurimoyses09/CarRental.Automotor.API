using CarRental.Automotor.Application.Events;
using CarRental.Automotor.Application.IRepository;
using CarRental.Automotor.Application.IService;
using CarRental.Automotor.Domain.Entities;

namespace CarRental.Automotor.Application.Service
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IRentalRepository _rentalRepository;
        private readonly IPaymentEventPublisher _eventPublisher;

        public PaymentService(
            IPaymentRepository paymentRepository,
            IRentalRepository rentalRepository,
            IPaymentEventPublisher eventPublisher)
        {
            _paymentRepository = paymentRepository;
            _rentalRepository = rentalRepository;
            _eventPublisher = eventPublisher;
        }

        public async Task<Payment> CreateAsync(Payment payment)
        {
            if (payment.Amount <= 0)
                throw new ArgumentException("The payment amount must be greater than zero.");

            if (await _rentalRepository.GetByIdAsync(payment.RentalId) is null)
                throw new KeyNotFoundException($"Rental {payment.RentalId} was not found.");

            // Simulação: o pagamento é aprovado imediatamente.
            payment.Approve();

            var created = await _paymentRepository.CreateAsync(payment);

            await _eventPublisher.PublishPaymentProcessedAsync(new PaymentProcessedEvent(
                created.Id,
                created.RentalId,
                created.Amount,
                created.Status,
                created.PaidAt
            ));

            return created;
        }

        public async Task<Payment?> GetByIdAsync(int id)
            => await _paymentRepository.GetByIdAsync(id);
    }
}
