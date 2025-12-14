using Locadora.AutoMotors.Domain.Entities;

namespace Locadora.AutoMotors.Application.IService
{
    public interface IClienteService
    {
        Task<Cliente> CreateAsync(Cliente cliente);
        Task<Cliente> UpdateAsync(Cliente cliente);
        Task<Cliente> GetByIdAsync(int id);
    }
}
