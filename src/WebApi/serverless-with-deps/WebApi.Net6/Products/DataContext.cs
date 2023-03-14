using System;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Net6.Products;

public class DataContext : DbContext
{
    private readonly AppSettings _appSettings;

    public DataContext(AppSettings appSettings)
    {
        _appSettings = appSettings;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlServer(_appSettings.AsConnectionString());
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>()
            .HasKey(p => p.Id);
    }

    public DbSet<Product> Products { get; set; }
}