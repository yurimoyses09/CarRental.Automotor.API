using CarRental.Automotor.Application.IRepository;
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

        public async Task<Domain.Entities.Rental> CreateAsync(Domain.Entities.Rental obj)
        {
            await _context.Rentals.AddAsync(obj);
            await _context.SaveChangesAsync();

            return obj;
        }

        public async Task<int> DeleteByIdAsync(int id)
        {
            var entity = await _context.Rentals.FirstOrDefaultAsync(x => x.Id == id);
            if (entity is not null)
            {
                _context.Rentals.Remove(entity);
                _context.SaveChanges();

                return entity.Id;
            }

            return 0;
        }

        public Task<IEnumerable<Domain.Entities.Rental>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<Domain.Entities.Rental> GetByIdAsync(int id)
        {
            var entity = await _context.Rentals.FirstOrDefaultAsync(x => x.Id == id);

            if (entity is not null)
                return entity;
            return
                null;
        }

        public async Task<Domain.Entities.Rental> UpdateAsync(Domain.Entities.Rental obj)
        {
            var exist = await _context.Rentals.FirstOrDefaultAsync(x => x.Id == obj.Id);

            if (exist is null)
                return null;

            _context.Entry(exist).CurrentValues.SetValues(obj);
            await _context.SaveChangesAsync();

            return exist;
        }
    }
}
