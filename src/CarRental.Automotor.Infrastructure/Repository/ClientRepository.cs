using CarRental.Automotor.Application.IRepository;
using CarRental.Automotor.Domain.Entities;
using CarRental.Automotor.Domain.Exceptions;
using CarRental.Automotor.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Npgsql;

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
            _appContext.Clients.Add(obj);

            try
            {
                await _appContext.SaveChangesAsync();
            }
            catch (Exception ex) when (IsUniqueViolation(ex))
            {
                throw new BusinessException("A client with this document already exists.");
            }

            return obj;
        }

        public async Task<bool> ExistsByDocumentAsync(string document) =>
            await _appContext.Clients.AnyAsync(x => x.Document == document && x.DeleteAt == default);

        private static bool IsUniqueViolation(Exception ex) =>
            (ex as PostgresException ?? ex.InnerException as PostgresException)?.SqlState == "23505";

        public async Task<int> DeleteByIdAsync(int id) =>
            await _appContext.Clients.Where(x => x.Id == id)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(x => x.DeleteAt, DateTimeOffset.UtcNow));

        public async Task<IEnumerable<Client>> GetAllAsync() =>
            await _appContext.Clients.AsNoTracking().ToListAsync();

        public async Task<Client?> GetByIdAsync(int id) =>
            await _appContext.Clients.FindAsync(id);

        public async Task<int> UpdateAsync(Client entity)
        {
            try
            {
                return await _appContext.Clients.Where(x => x.Id == entity.Id)
                    .ExecuteUpdateAsync(s => s
                        .SetProperty(p => p.UpdatedAt, DateTimeOffset.UtcNow)
                        .SetProperty(p => p.Name, entity.Name)
                        .SetProperty(p => p.Document, entity.Document)
                        .SetProperty(p => p.Age, entity.Age));
            }
            catch (Exception ex) when (IsUniqueViolation(ex))
            {
                throw new BusinessException("A client with this document already exists.");
            }
        }
    }
}
