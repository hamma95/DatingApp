using System.Data;
using System.Data.Common;
using API.Data;
using API.Entities;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Tests.Integration;

class TestDatabaseManager : IDisposable
{
    private readonly DatabaseCredentials _databaseCredentials;
    private readonly DataContext _dataContext;

    public TestDatabaseManager()
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile(TestHelpers.GetAppSettingsForTestsFilePath(), optional: false)
            .AddEnvironmentVariables()
            .Build();
        _databaseCredentials = configuration.GetSection(nameof(DatabaseCredentials)).Get<DatabaseCredentials>();
        var optionsBuilder = new DbContextOptionsBuilder<DataContext>().UseSqlite(_databaseCredentials.GetConnectionString());
        _dataContext = new DataContext(optionsBuilder.Options);
    }

    public void RunMigrations()
    {
        _dataContext.Database.Migrate();
    }
    
    public void CleanDatabase()
    {
        using var transaction = _dataContext.Database.BeginTransaction();
        try
        {
            // Add your entity types here, ordered by deletion order respecting foreign key constraints
            var tableNames = new List<string>
            {
                "Users",
            };

            // disable foreign key checks for better perf
            _dataContext.Database.ExecuteSqlRaw("PRAGMA foreign_keys = OFF;");

            foreach (var tableName in tableNames)
            {
                _dataContext.Database.ExecuteSqlRaw($"DELETE FROM {tableName};");
            }

            // Optionally reset the SQLite sequence counters
            foreach (var tableName in tableNames)
            {
                _dataContext.Database.ExecuteSqlRaw($"DELETE FROM sqlite_sequence WHERE name = '{tableName}';");
            }

            // Re-enable foreign key checks
            _dataContext.Database.ExecuteSqlRaw("PRAGMA foreign_keys = ON;");

            transaction.Commit();
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }

    public void Dispose()
    {
        _dataContext?.Dispose();
    }
}