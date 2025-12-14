using Locadora.AutoMotors.Application.IRepository;
using Locadora.AutoMotors.Application.IService;
using Locadora.AutoMotors.Application.Service;
using Locadora.AutoMotors.Infrastructure.Context;
using Locadora.AutoMotors.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IClienteService, ClienteService>();
builder.Services.AddScoped<IClienteRepository, ClienteRepository>();


// String de conexão PostgreSQL
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Registrar DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

    for (var retry = 1; retry <= 5; retry++)
    {
        try
        {
            db.Database.Migrate();
            logger.LogInformation("Migrations aplicadas com sucesso");
            break;
        }
        catch (Exception ex)
        {
            logger.LogWarning("Tentativa {Retry} falhou: {Message}", retry, ex.Message);
            Thread.Sleep(5000);
        }
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
