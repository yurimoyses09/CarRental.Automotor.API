using CarRental.Automotor.Application.IRepository;
using CarRental.Automotor.Domain.Entities;
using CarRental.Automotor.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace CarRental.Automotor.Infrastructure.Repository
{
    public class AutomobileRepository : IAutomobileRepository
    {
        private readonly AppDbContext _context;

        public AutomobileRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Automobile> CreateAsync(Automobile obj)
        {
            await _context.Automobiles.AddAsync(obj);
            await _context.SaveChangesAsync();

            return obj;
        }

        public async Task<int> DeleteByIdAsync(int id)
        {
            var item = await _context.Automobiles.FirstOrDefaultAsync(x => x.Id == id);

            _context.Automobiles.Remove(item);
            return await _context.SaveChangesAsync();
        }

        public Task<IEnumerable<Automobile>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<Automobile> GetByIdAsync(int id)
        {
            var obj = await _context.Automobiles.FirstOrDefaultAsync(x => x.Id == id);

            if (obj == null)
                return null;

            return obj;
        }

        public Task<Automobile> UpdateAsync(Automobile entity)
        {
            throw new NotImplementedException();
        }
    }
}
