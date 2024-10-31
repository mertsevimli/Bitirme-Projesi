using Microsoft.EntityFrameworkCore;
using Sarideniz.Core.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace Sarideniz.Data.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.Property(x => x.Name).HasMaxLength(150);
        builder.Property(x => x.Description).HasMaxLength(500);
        builder.Property(x => x.Image).HasMaxLength(100);
        builder.Property(x => x.ProductCode).HasMaxLength(50);
    }
}