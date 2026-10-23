using CarRental.Automotor.Domain.Entities;

namespace CarRental.Automotor.Application.IService
{
    public interface IPaymentService
    {
        Task<Payment> CreateAsync(Payment payment);
        Task<Payment?> GetByIdAsync(int id);
    }
}
