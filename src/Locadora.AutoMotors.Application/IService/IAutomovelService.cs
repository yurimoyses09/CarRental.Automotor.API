using Locadora.AutoMotors.Domain.Entities;

namespace Locadora.AutoMotors.Application.IService
{
    public interface IAutomovelService
    {
        Task<Automovel> CreateAsync(Automovel automovel);
        Task<Automovel> UpdateAsync(Automovel automovel);
        Task<Automovel> GetByIdAsync(int id);
    }
}
