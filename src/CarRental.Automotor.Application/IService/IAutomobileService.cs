using CarRental.Automotor.Domain.Entities;

namespace CarRental.Automotor.Application.IService
{
    public interface IAutomobileService
    {
        Task<Automobile> CreateAsync(Automobile Automobile);
        Task<Automobile> UpdateAsync(Automobile Automobile);
        Task<Automobile> GetByIdAsync(int id);
    }
}
