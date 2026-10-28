using CarRental.Automotor.Domain.Entities;

namespace CarRental.Automotor.Application.IRepository
{
    public interface IPaymentRepository : IBaseRepository<Payment>
    {
        Task<bool> ExistsApprovedByRentalIdAsync(int rentalId);
    }
}
