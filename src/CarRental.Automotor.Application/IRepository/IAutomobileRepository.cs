using CarRental.Automotor.Domain.Entities;

namespace CarRental.Automotor.Application.IRepository
{
    public interface IAutomobileRepository : IBaseRepository<Automobile>
    {
        Task<bool> ExistsByPlateAsync(string plate);
    }
}
