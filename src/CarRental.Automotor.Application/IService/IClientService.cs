using CarRental.Automotor.Domain.Entities;

namespace CarRental.Automotor.Application.IService
{
    public interface IClientService
    {
        Task<Client> CreateAsync(Client cliente);
        Task<int> UpdateAsync(Client cliente);
        Task<Client?> GetByIdAsync(int id);
        Task<int> DeleteByIdAsync(int id);
     }
}
