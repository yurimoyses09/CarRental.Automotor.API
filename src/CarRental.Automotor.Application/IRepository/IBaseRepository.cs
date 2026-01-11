namespace CarRental.Automotor.Application.IRepository
{
    public interface IBaseRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetByIdAsync(int id);
        Task<T> CreateAsync(T obj);
        Task<int> UpdateAsync(T entity);
        Task<int> DeleteByIdAsync(int id);
    }
}
