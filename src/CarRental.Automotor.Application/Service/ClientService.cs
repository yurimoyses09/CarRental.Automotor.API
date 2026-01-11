using CarRental.Automotor.Application.IRepository;
using CarRental.Automotor.Application.IService;
using CarRental.Automotor.Domain.Entities;

namespace CarRental.Automotor.Application.Service
{
    public class ClientService : IClientService
    {
        private readonly IClientRepository _clientRepository;

        public ClientService(IClientRepository clienteRepository)
        {
            _clientRepository = clienteRepository;
        }

        public async Task<Client> CreateAsync(Client cliente) 
            => await _clientRepository.CreateAsync(cliente);

        public async Task<int> DeleteByIdAsync(int id) =>
            await _clientRepository.DeleteByIdAsync(id);

        public async Task<IEnumerable<Client>> GetAllAsync() => 
            await _clientRepository.GetAllAsync();

        public async Task<Client?> GetByIdAsync(int id) 
            => await _clientRepository.GetByIdAsync(id);

        public async Task<int> UpdateAsync(Client cliente) 
            => await _clientRepository.UpdateAsync(cliente);
    }
}
