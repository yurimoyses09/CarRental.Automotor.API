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
        public DbSet<Payment> Payments => Set<Payment>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Um aluguel só pode ter um pagamento aprovado (não excluído).
            modelBuilder.Entity<Payment>()
                .HasIndex(p => p.RentalId)
                .IsUnique()
                .HasFilter("payment_status = 2 AND delete_at = '0001-01-01 00:00:00+00'");

            // Documento de cliente e placa de automóvel não podem se repetir entre registros não excluídos.
            modelBuilder.Entity<Client>()
                .HasIndex(c => c.Document)
                .IsUnique()
                .HasFilter("delete_at = '0001-01-01 00:00:00+00'");

            modelBuilder.Entity<Automobile>()
                .HasIndex(a => a.Plate)
                .IsUnique()
                .HasFilter("delete_at = '0001-01-01 00:00:00+00'");
        }
    }
}
