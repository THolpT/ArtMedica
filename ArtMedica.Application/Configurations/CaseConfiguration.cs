using ArtMedica.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ArtMedica.Application.Configurations;

public class CaseConfiguration : IEntityTypeConfiguration<Case>
{
    public void Configure(EntityTypeBuilder<Case> builder)
    {
        builder.ToTable("Cases");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.ClientName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(c => c.Problem)
            .IsRequired()
            .HasMaxLength(2000);

        builder.Property(c => c.Solution)
            .IsRequired()
            .HasMaxLength(2000);

        builder.Property(c => c.Date)
            .IsRequired();

        builder.Property(c => c.Review)
            .IsRequired();
    }
}