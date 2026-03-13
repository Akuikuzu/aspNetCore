using BApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BApi.Data;

public class DbContextBApi : DbContext
{
    public DbContextBApi(DbContextOptions<DbContextBApi> options) : base(options)
    {
    }

    public DbSet<Product> Products => Set<Product>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>()
            .ToTable("Products")
            .HasKey(p => p.Id);
    }
}