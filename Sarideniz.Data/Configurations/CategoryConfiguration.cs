using Microsoft.EntityFrameworkCore;
using Sarideniz.Core.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Sarideniz.Data.Configurations;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.Property(x => x.Name).IsRequired().HasMaxLength(50);
        builder.Property(x => x.Image).HasMaxLength(50);
        builder.HasData(
            new Category
            {
                Id = 1,
                Name = "Küpe",
                IsActive = true,
                IsTopMenu = true,
                ParentId = 0,
                OrderNo = 1,
            },
            new Category
            {
                Id = 2,
                Name = "Kolye",
                IsActive = true,
                IsTopMenu = true,
                ParentId = 0,
                OrderNo = 2,
            }
        );
    }
}