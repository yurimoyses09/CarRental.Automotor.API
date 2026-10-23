using CarRental.Automotor.Application.IRepository;
using CarRental.Automotor.Domain.Entities;
using CarRental.Automotor.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace CarRental.Automotor.Infrastructure.Repository
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly AppDbContext _context;

        public PaymentRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Payment> CreateAsync(Payment obj)
        {
            await _context.Payments.AddAsync(obj);
            await _context.SaveChangesAsync();

            return obj;
        }

        public async Task<int> DeleteByIdAsync(int id) =>
            await _context.Payments.Where(x => x.Id == id)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(p => p.DeleteAt, DateTimeOffset.UtcNow));

        public async Task<IEnumerable<Payment>> GetAllAsync() =>
            await _context.Payments.AsNoTracking().ToListAsync();

        public async Task<Payment?> GetByIdAsync(int id) =>
            await _context.Payments.FindAsync(id);

        public async Task<int> UpdateAsync(Payment obj) =>
            await _context.Payments
                .Where(x => x.Id == obj.Id)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(x => x.RentalId, obj.RentalId)
                    .SetProperty(x => x.Amount, obj.Amount)
                    .SetProperty(x => x.Status, obj.Status)
                    .SetProperty(x => x.PaidAt, obj.PaidAt)
                    .SetProperty(x => x.UpdatedAt, DateTimeOffset.UtcNow));
    }
}
