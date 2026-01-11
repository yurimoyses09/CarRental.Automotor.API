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
            _context.Automobiles.Add(obj);
            await _context.SaveChangesAsync();

            return obj;
        }

        public async Task<int> DeleteByIdAsync(int id) =>
            await _context.Automobiles.Where(x => x.Id == id)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(p => p.DeleteAt, DateTimeOffset.UtcNow));

        public async Task<IEnumerable<Automobile>> GetAllAsync() =>     
            await _context.Automobiles.AsNoTracking().ToListAsync();

        public async Task<Automobile?> GetByIdAsync(int id) => 
            await _context.Automobiles.FindAsync(id);

        public async Task<int> UpdateAsync(Automobile entity) =>
            await _context.Automobiles.Where(x => x.Id == entity.Id)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(p => p.YearManufacture, entity.YearManufacture)
                    .SetProperty(p => p.FuelType, entity.FuelType)
                    .SetProperty(p => p.UpdatedAt, DateTimeOffset.UtcNow)
                    .SetProperty(p => p.Color, entity.Color)
                    .SetProperty(p => p.Plate, entity.Plate));
    }
}
