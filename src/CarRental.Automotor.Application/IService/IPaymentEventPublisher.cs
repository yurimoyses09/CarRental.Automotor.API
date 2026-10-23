using CarRental.Automotor.Application.Events;

namespace CarRental.Automotor.Application.IService
{
    public interface IPaymentEventPublisher
    {
        Task PublishPaymentProcessedAsync(PaymentProcessedEvent paymentEvent);
    }
}
