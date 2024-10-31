using Microsoft.EntityFrameworkCore;
using Sarideniz.Core.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Sarideniz.Data.Configurations;

public class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
{
    public void Configure(EntityTypeBuilder<AppUser> builder)
    {
        builder.Property(x => x.Name).IsRequired().HasColumnType("varchar(50)").HasMaxLength(50);
        builder.Property(x => x.Surname).IsRequired().HasColumnType("varchar(50)").HasMaxLength(50);
        builder.Property(x => x.Email).IsRequired().HasColumnType("varchar(50)").HasMaxLength(50);
        builder.Property(x => x.Phone).HasColumnType("varchar(50)").HasMaxLength(50);
        builder.Property(x => x.Password).IsRequired().HasColumnType("nvarchar(50)").HasMaxLength(50);
        builder.Property(x => x.UserName).HasColumnType("nvarchar(50)").HasMaxLength(50);

        new AppUser
        {
            Id = 1,
            CreateDate = DateTime.Now,
            UserName = "Admin",
            Email = "admin@sarideniz.com",
            IsActive = true,
            IsAdmin =  true,
            Name = "Admin",
            Password = "123456*",
            Surname = "Test User",
            
            
            
        };
    }
}