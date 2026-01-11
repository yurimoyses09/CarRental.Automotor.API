using CarRental.Automotor.Application.IRepository;
using CarRental.Automotor.Application.IService;

namespace CarRental.Automotor.Application.Service
{
    public class RentalService : IRentalService
    {
        private readonly IRentalRepository _repository;

        public RentalService(IRentalRepository repository)
        {
            _repository = repository;
        }

        public async Task<Domain.Entities.Rental> CreateAsync(Domain.Entities.Rental locadora)
            => await _repository.CreateAsync(locadora);

        public async Task<Domain.Entities.Rental> GetByIdAsync(int id)
            => await _repository.GetByIdAsync(id);

        public async Task<Domain.Entities.Rental> UpdateAsync(Domain.Entities.Rental locadora)
            => await _repository.UpdateAsync(locadora);
    }
}
