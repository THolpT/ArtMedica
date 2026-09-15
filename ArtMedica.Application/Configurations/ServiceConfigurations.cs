using ArtMedica.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ArtMedica.Infrastructure.Configurations;

public class ServiceConfiguration : IEntityTypeConfiguration<Service>
{
    public void Configure(EntityTypeBuilder<Service> builder)
    {
        builder.ToTable("Services");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.NomenclatureCode)
            .HasMaxLength(50);

        builder.Property(s => s.MISCode)
            .HasMaxLength(50);

        builder.Property(s => s.Duration)
            .IsRequired();

        builder.Property(s => s.Price)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(s => s.DisplayTitle)
            .IsRequired()
            .HasMaxLength(300);

        builder.Property(s => s.TechnicalTitle)
            .HasMaxLength(300);

        builder.Property(s => s.Description)
            .HasMaxLength(5000);

        builder.Property(s => s.Preparation)
            .HasMaxLength(5000);

        builder.HasOne(s => s.Category)
            .WithMany(c => c.Services)
            .HasForeignKey("CategoryId")
            .OnDelete(DeleteBehavior.Restrict);
    }
}