using CarRental.Automotor.Application.IRepository;
using CarRental.Automotor.Domain.Entities;
using CarRental.Automotor.Domain.Exceptions;
using CarRental.Automotor.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Npgsql;

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

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex) when (IsUniqueViolation(ex))
            {
                throw new BusinessException("An automobile with this plate already exists.");
            }

            return obj;
        }

        public async Task<bool> ExistsByPlateAsync(string plate) =>
            await _context.Automobiles.AnyAsync(x => x.Plate == plate && x.DeleteAt == default);

        private static bool IsUniqueViolation(Exception ex) =>
            (ex as PostgresException ?? ex.InnerException as PostgresException)?.SqlState == "23505";

        public async Task<int> DeleteByIdAsync(int id) =>
            await _context.Automobiles.Where(x => x.Id == id)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(p => p.DeleteAt, DateTimeOffset.UtcNow));

        public async Task<IEnumerable<Automobile>> GetAllAsync() =>     
            await _context.Automobiles.AsNoTracking().ToListAsync();

        public async Task<Automobile?> GetByIdAsync(int id) => 
            await _context.Automobiles.FindAsync(id);

        public async Task<int> UpdateAsync(Automobile entity)
        {
            try
            {
                return await _context.Automobiles.Where(x => x.Id == entity.Id)
                    .ExecuteUpdateAsync(s => s
                        .SetProperty(p => p.YearManufacture, entity.YearManufacture)
                        .SetProperty(p => p.FuelType, entity.FuelType)
                        .SetProperty(p => p.UpdatedAt, DateTimeOffset.UtcNow)
                        .SetProperty(p => p.Color, entity.Color)
                        .SetProperty(p => p.Plate, entity.Plate));
            }
            catch (Exception ex) when (IsUniqueViolation(ex))
            {
                throw new BusinessException("An automobile with this plate already exists.");
            }
        }
    }
}
