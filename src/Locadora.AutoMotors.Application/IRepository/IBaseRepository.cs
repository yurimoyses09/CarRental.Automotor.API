using System.Collections.Generic;

namespace Locadora.AutoMotors.Application.IRepository
{
    public interface IBaseRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAll();
        Task<T> GetById(int id);
        Task<T> Create(T obj);
        Task<T> Update(T entity);
        Task<int> DeleteById(int id);
    }
}
