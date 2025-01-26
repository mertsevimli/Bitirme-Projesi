using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Options;
using Sarideniz.Core.Entities;
using Sarideniz.Data.Configurations;

namespace Sarideniz.Data;

public class DatabaseContext : DbContext
{
    public DbSet<Address> Addresses { get; set; }
    public DbSet<AppUser> AppUsers { get; set; }
    public DbSet<Brand> Brands { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Contact> Contacts { get; set; }
    public DbSet<News> News { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Slider> Sliders { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderLine> OrderLines { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(@"Server=localhost;Database=SaridenizDb;User Id=SA;Password=Lebron234gizem;TrustServerCertificate=True;");
        
        // optionsBuilder.ConfigureWarnings(warnings => warnings.Ignore
        //     (RelationalEventId.PendingModelChangesWarning));
        
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      
        
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly()); // çalışkan dll in içinden bul
        base.OnModelCreating(modelBuilder);
    }
}