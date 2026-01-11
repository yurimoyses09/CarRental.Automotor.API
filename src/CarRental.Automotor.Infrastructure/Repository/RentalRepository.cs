using CarRental.Automotor.Application.IRepository;
using CarRental.Automotor.Domain.Entities;
using CarRental.Automotor.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace CarRental.Automotor.Infrastructure.Repository
{
    public class RentalRepository : IRentalRepository
    {
        private readonly AppDbContext _context;

        public RentalRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Rental> CreateAsync(Rental obj)
        {
            await _context.Rentals.AddAsync(obj);
            await _context.SaveChangesAsync();

            return obj;
        }

        public async Task<int> DeleteByIdAsync(int id) => 
            await _context.Rentals.Where(x => x.Id == id)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(p => p.DeleteAt, DateTimeOffset.UtcNow));

        public async Task<IEnumerable<Rental>> GetAllAsync() =>
            await _context.Rentals.AsNoTracking().ToListAsync();

        public async Task<Rental?> GetByIdAsync(int id) => 
            await _context.Rentals.FindAsync(id);
        
        public async Task<int> UpdateAsync(Rental obj) => 
            await _context.Rentals
                    .Where(x => x.Id == obj.Id)
                    .ExecuteUpdateAsync(setters => setters
                        .SetProperty(x => x.ClientId, obj.ClientId)
                        .SetProperty(x => x.AutomobileId, obj.AutomobileId)
                        .SetProperty(x => x.StartDate, obj.StartDate)
                        .SetProperty(x => x.ExpectedEndDate, obj.ExpectedEndDate)
                        .SetProperty(x => x.ActualEndDate, obj.ActualEndDate)
                        .SetProperty(x => x.DailyRate, obj.DailyRate)
                        .SetProperty(x => x.TotalAmount, obj.TotalAmount)
                        .SetProperty(x => x.Status, obj.Status)
                        .SetProperty(x => x.UpdatedAt, DateTimeOffset.UtcNow)
                    );
    }
}
