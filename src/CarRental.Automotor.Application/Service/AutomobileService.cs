using CarRental.Automotor.Application.IRepository;
using CarRental.Automotor.Application.IService;
using CarRental.Automotor.Domain.Entities;
using CarRental.Automotor.Domain.Exceptions;

namespace CarRental.Automotor.Application.Service
{
    public class AutomobileService : IAutomobileService
    {
        private readonly IAutomobileRepository _repository;

        public AutomobileService(IAutomobileRepository repository)
        {
            _repository = repository;
        }

        public async Task<Automobile> CreateAsync(Automobile Automobile)
        {
            if (await _repository.ExistsByPlateAsync(Automobile.Plate))
                throw new BusinessException("An automobile with this plate already exists.");

            return await _repository.CreateAsync(Automobile);
        }

        public async Task<int> DeleteByIdAsync(int id)
            => await _repository.DeleteByIdAsync(id);

        public async Task<Automobile?> GetByIdAsync(int id) 
            => await _repository.GetByIdAsync(id);

        public async Task<int> UpdateAsync(Automobile Automobile) 
            => await _repository.UpdateAsync(Automobile);
    }
}
