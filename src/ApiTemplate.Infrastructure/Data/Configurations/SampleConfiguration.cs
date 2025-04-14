using ApiTemplate.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApiTemplate.Infrastructure.Data.Configurations;

/// <summary>
/// Configuration for the Sample entity
/// </summary>
public class SampleConfiguration : IEntityTypeConfiguration<Sample>
{
    /// <summary>
    /// Configures the Sample entity
    /// </summary>
    /// <param name="builder">Entity type builder</param>
    public void Configure(EntityTypeBuilder<Sample> builder)
    {
        builder.HasKey(s => s.Id);

        builder.Property(s => s.Name)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(s => s.Description)
            .HasMaxLength(500);

        // Configure the enum as a string for better readability in the database
        builder.Property(s => s.Status)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        // Configure indexes
        builder.HasIndex(s => s.Name);
    }
}