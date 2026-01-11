using CarRental.Automotor.Application.IRepository;
using CarRental.Automotor.Application.IService;
using CarRental.Automotor.Application.Service;
using CarRental.Automotor.Infrastructure.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace CarRental.Automotor.Infrastructure.DependencyInjection
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<IClientService, ClientService>();
            services.AddScoped<IClientRepository, ClientRepository>();

            services.AddScoped<IAutomobileRepository, AutomobileRepository>();
            services.AddScoped<IAutomobileService, AutomobileService>();

            services.AddScoped<IRentalService, RentalService>();
            services.AddScoped<IRentalRepository, RentalRepository>();

            return services;

        }
    }
}
