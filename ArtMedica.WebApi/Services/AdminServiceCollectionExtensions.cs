using ArtMedica.WebApi.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ArtMedica.WebApi.Services;

public static class AdminServiceCollectionExtensions
{
    public static IServiceCollection AddAdmin<TDbContext>(
        this IServiceCollection services,
        Action<AdminBuilder> configure)
        where TDbContext : DbContext
    {
        var registry = new AdminRegistry();

        var builder = new AdminBuilder(registry);

        configure(builder);

        services.AddSingleton<IAdminRegistry>(registry);

        services.AddScoped<IAdminCrudService,
            AdminCrudService<TDbContext>>();

        return services;
    }
}