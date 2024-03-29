using API.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Tests.Integration;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    public readonly HttpClient HttpClient;

    public CustomWebApplicationFactory()
    {
        HttpClient = CreateDefaultClient()!;
    }
    

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("IntegrationTests");
        
        builder.ConfigureAppConfiguration((context, configBuilder) =>
        {
            configBuilder.AddJsonFile(TestHelpers.GetAppSettingsForTestsFilePath(), optional: false);
        });
        
        builder.ConfigureServices((services) =>
        {
            var serviceProvider = services.BuildServiceProvider();
            using var scope = serviceProvider.CreateScope();
            var migrationRespository = scope.ServiceProvider.GetRequiredService<IMigrationRepository>();
            migrationRespository.MigrateDatabase();
        });
    }
}