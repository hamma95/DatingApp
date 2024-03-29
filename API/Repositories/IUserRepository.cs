using API.Entities;
namespace API.Repositories;

public interface IUserRepository
{
    public Task<AppUser> GetUser(int id);
    public Task<IEnumerable<AppUser>> GetUsers(UserFilters userFilters);
    public Task<AppUser> CreateUser(AppUser user);
    public Task<int> SaveChanges();
}
