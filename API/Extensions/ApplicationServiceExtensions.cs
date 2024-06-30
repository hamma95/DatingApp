using API.Data;
using API.Entities;
using API.Interfaces;
using API.Repositories;
using API.Services;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions;

public static class ApplicationServiceExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<DataContext>(options =>
        {
            var databaseCredentials = config.GetSection(nameof(DatabaseCredentials)).Get<DatabaseCredentials>();
            options.UseSqlite(databaseCredentials.GetConnectionString());
        });
        services.Configure<DatabaseCredentials>(config.GetSection(key: nameof(DatabaseCredentials)));
        
        services.AddScoped<IUserRepository, EfCoreUserRepository>();
        services.AddScoped<IMigrationRepository, EfCoreMigrationRepository>();
        services.AddScoped<UserService>();
        services.AddScoped<ITokenService, TokenService>();

        return services;
    }
}
