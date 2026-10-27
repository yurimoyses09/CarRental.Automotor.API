using CarRental.Automotor.Application.IRepository;
using CarRental.Automotor.Domain.Entities;
using CarRental.Automotor.Domain.Enums;
using CarRental.Automotor.Domain.Exceptions;
using CarRental.Automotor.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Npgsql;

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

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex) when (ex.InnerException is PostgresException { SqlState: "23505" })
            {
                // Violação do índice único: outra requisição pagou o aluguel primeiro.
                throw new BusinessException("This rental has already been paid.");
            }

            return obj;
        }

        public async Task<bool> ExistsApprovedByRentalIdAsync(int rentalId) =>
            await _context.Payments.AnyAsync(p =>
                p.RentalId == rentalId && p.Status == PaymentStatus.Approved);

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
