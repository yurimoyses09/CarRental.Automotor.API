using CarRental.Automotor.Application.IRepository;
using CarRental.Automotor.Application.IService;
using CarRental.Automotor.Domain.Entities;

namespace CarRental.Automotor.Application.Service
{
    public class RentalService : IRentalService
    {
        private readonly IRentalRepository _repository;

        public RentalService(IRentalRepository repository)
        {
            _repository = repository;
        }

        public async Task<Rental> CreateAsync(Rental rental)
            => await _repository.CreateAsync(rental);

        public Task<int> DeleteByIdAsync(int id)
            => _repository.DeleteByIdAsync(id);
            
        public async Task<Rental?> GetByIdAsync(int id)
            => await _repository.GetByIdAsync(id);

        public async Task<int> UpdateAsync(Rental rental)
            => await _repository.UpdateAsync(rental);
    }
}
