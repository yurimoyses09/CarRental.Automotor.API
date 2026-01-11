using CarRental.Automotor.Domain.Entities;

namespace CarRental.Automotor.Application.IService
{
    public interface IRentalService
    {
        Task<Rental> CreateAsync(Rental locadora);
        Task<Rental> UpdateAsync(Rental locadora);
        Task<Rental> GetByIdAsync(int id);
    }
}
