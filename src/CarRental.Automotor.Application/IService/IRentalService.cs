using CarRental.Automotor.Domain.Entities;

namespace CarRental.Automotor.Application.IService
{
    public interface IRentalService
    {
        Task<Rental> CreateAsync(Rental rental);
        Task<int> UpdateAsync(Rental rental);
        Task<Rental?> GetByIdAsync(int id);
        Task<int> DeleteByIdAsync(int id);
    }
}
