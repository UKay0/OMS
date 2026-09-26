using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OMS.Domain.Entities;

namespace OMS.Infrastructure.Persistence.Configurations;

/// <summary>EF Core mapping for Employee. Email is unique so the Application-layer "no duplicate email" rule has a matching DB constraint.</summary>
public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.Property(e => e.Name).HasMaxLength(100).IsRequired();
        builder.Property(e => e.Email).HasMaxLength(150).IsRequired();
        builder.HasIndex(e => e.Email).IsUnique();
        builder.Property(e => e.Department).HasMaxLength(100).IsRequired();
        builder.Property(e => e.Role).HasMaxLength(50).IsRequired();
    }
}
