using BIT.Domain.Interfaces;
using BIT.Infrastructure.Persistence;
using BIT.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BIT.Infrastructure;

public static class InfrastructureServiceExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        // Register DbContext
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql("Host=localhost;Database=InvoiceTrackerDB;Username=postgres;Password=P4ssw0rd_25"));

        // Register Generic Repository
        services.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));

        return services;
    }
}
