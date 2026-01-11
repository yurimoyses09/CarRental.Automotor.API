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

        public Task<int> DeleteByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Client>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<Client> GetByIdAsync(int id) 
            => await _clientRepository.GetByIdAsync(id);

        public async Task<Client> UpdateAsync(Client cliente) 
            => await _clientRepository.UpdateAsync(cliente);
    }
}
