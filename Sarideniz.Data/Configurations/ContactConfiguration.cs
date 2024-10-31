using Microsoft.EntityFrameworkCore;
using Sarideniz.Core.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Sarideniz.Data.Configurations;

public class ContactConfiguration : IEntityTypeConfiguration<Contact>
{
    public void Configure(EntityTypeBuilder<Contact> builder)
    {
        builder.Property(x => x.Name).IsRequired().HasMaxLength(50);
        builder.Property(x => x.Surname).IsRequired().HasMaxLength(50);
        builder.Property(x => x.Email).HasMaxLength(50);
        builder.Property(x => x.Phone).HasColumnType("varchar(50)").HasMaxLength(50);
        builder.Property(x => x.Message).IsRequired().HasMaxLength(500);
    }
}