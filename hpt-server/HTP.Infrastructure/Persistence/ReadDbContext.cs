using HTP.Infrastructure.Persistence.Users.Read;
using Microsoft.EntityFrameworkCore;

namespace HTP.Infrastructure.Persistence;

public sealed class ReadDbContext : DbContext
{
    public DbSet<UserReadModel> Users { get; set; }

    public ReadDbContext(DbContextOptions<ReadDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserReadModelConfiguration());

        base.OnModelCreating(modelBuilder);
    }
}
