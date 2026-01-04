using Locadora.AutoMotors.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Locadora.AutoMotors.Infrastructure.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Cliente> Clientes => Set<Cliente>();
        public DbSet<Automovel> Automoveis => Set<Automovel>();
        public DbSet<Domain.Entities.Locadora> Locadoras => Set<Domain.Entities.Locadora>();

    }
}
