using API.Repositories;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class EfCoreMigrationRepository : IMigrationRepository
{
    private readonly DataContext dataContext;

    public EfCoreMigrationRepository(DataContext dataContext)
    {
        this.dataContext = dataContext;
    }
    
    public bool MigrateDatabase()
    { 
        dataContext.Database.Migrate();
        return true;
    }
}