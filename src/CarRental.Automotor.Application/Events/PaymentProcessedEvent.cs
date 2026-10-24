using CarRental.Automotor.Domain.Enums;

namespace CarRental.Automotor.Application.Events
{
    public record PaymentProcessedEvent(
        int PaymentId,
        int RentalId,
        decimal Amount,
        PaymentStatus Status,
        DateTimeOffset? PaidAt
    );
}
