using System;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Net6.Products;

public class DataContext : DbContext
{
    private readonly DatabaseConnectionDetails _databaseConnectionDetails;

    public DataContext(DatabaseConnectionDetails databaseConnectionDetails)
    {
        _databaseConnectionDetails = databaseConnectionDetails;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlServer(_databaseConnectionDetails.AsConnectionString());
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>()
            .HasKey(p => p.Id);
    }

    public DbSet<Product> Products { get; set; }
}