using Core.Entities;
using Data.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Data.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Contact> Contacts => Set<Contact>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ContactConfiguration());

        base.OnModelCreating(modelBuilder);
    }
}
