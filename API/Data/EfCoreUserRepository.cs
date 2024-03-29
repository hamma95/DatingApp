using API.Entities;
using API.Repositories;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class EfCoreUserRepository: IUserRepository
{
    private readonly DataContext dataContext;

    public EfCoreUserRepository(DataContext dataContext)
    {
        this.dataContext = dataContext;
    }
    
    public async Task<AppUser> GetUser(int id)
    {
        return await dataContext.Users.FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<IEnumerable<AppUser>> GetUsers(UserFilters userFilters)
    {
        if (userFilters is null)
            return Enumerable.Empty<AppUser>();
        
        var query = dataContext.Users.AsQueryable();
        
        
        if (!string.IsNullOrWhiteSpace(userFilters.Mail))
        {
            query = query.Where(user => userFilters.Mail.Equals(user.Mail));
        }

        if (!string.IsNullOrWhiteSpace(userFilters.FirstName))
        {
            query = query.Where(user => userFilters.FirstName.Equals(user.FirstName));
        }

        if (!string.IsNullOrWhiteSpace(userFilters.LastName))
        {           
            query = query.Where(user => userFilters.LastName.Equals(user.LastName));
        }
        
        return await query.ToListAsync(); 
    }

    public Task<AppUser> CreateUser(AppUser user)
    {
        var userEntry = dataContext.Add(user);
        return Task.FromResult(userEntry.Entity);
    }

    public async Task<int> SaveChanges()
    {
        return await dataContext.SaveChangesAsync();
    }
}