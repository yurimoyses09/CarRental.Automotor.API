using CarRental.Automotor.Application.IRepository;
using CarRental.Automotor.Domain.Entities;
using CarRental.Automotor.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace CarRental.Automotor.Infrastructure.Repository
{
    public class ClientRepository : IClientRepository
    {
        private readonly AppDbContext _appContext;

        public ClientRepository(AppDbContext appContext)
        {
            _appContext = appContext;
        }

        public async Task<Client> CreateAsync(Client obj)
        {
            await _appContext.Clients.AddAsync(obj);
            await _appContext.SaveChangesAsync();

            return obj;
        }

        public Task<int> DeleteByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Client>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<Client> GetByIdAsync(int id)
        {
            var entry = await _appContext.Clients.FirstOrDefaultAsync(x => x.Id == id);

            if (entry == null) 
                return null;

            return entry;
        }

        public async Task<Client> UpdateAsync(Client entity)
        {
            var exist = await _appContext.Clients.FirstOrDefaultAsync(x => x.Id == entity.Id);

            if (exist is null)
                return null;

            _appContext.Entry(exist).CurrentValues.SetValues(entity);
            await _appContext.SaveChangesAsync();

            return exist;
        }
    }
}
