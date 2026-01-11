using CarRental.Automotor.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CarRental.Automotor.Infrastructure.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Client> Clients => Set<Client>();
        public DbSet<Automobile> Automobiles => Set<Automobile>();
        public DbSet<Rental> Rentals => Set<Rental>();
    }
}
