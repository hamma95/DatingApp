using API.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        SetupUserEntityConstraints(modelBuilder.Entity<AppUser>());
    }

    private static void SetupUserEntityConstraints(EntityTypeBuilder<AppUser> userEntityBuilder)
    {
        userEntityBuilder.Property(user => user.Mail).IsRequired();
        userEntityBuilder.Property(user => user.PasswordHash).IsRequired();
        userEntityBuilder.Property(user => user.PasswordSalt).IsRequired();
        // userEntityBuilder.Property(user => user.FirstName).IsRequired();
        // userEntityBuilder.Property(user => user.LastName).IsRequired();

        userEntityBuilder.HasIndex(user => user.Mail).IsUnique();
    }

    public DbSet<AppUser> Users { get; set; }
}
