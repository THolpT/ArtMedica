using ArtMedica.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ArtMedica.Infrastructure.Configurations;

public class ServiceConfiguration : IEntityTypeConfiguration<Service>
{
    public void Configure(EntityTypeBuilder<Service> builder)
    {
        builder.ToTable("Services");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Price).HasColumnType("decimal(18,2)");
        builder.Property(x => x.NomenclatureCode).IsRequired().HasMaxLength(50);
        builder.Property(x => x.MISCode).IsRequired().HasMaxLength(50);
        builder.Property(x => x.DisplayTitle).IsRequired().HasMaxLength(200);
        builder.Property(x => x.TechnicalTitle).IsRequired().HasMaxLength(200);
        builder.Property(x => x.Description).HasMaxLength(2000);
        builder.Property(x => x.Preparation).HasMaxLength(2000);
        builder.Property(x => x.Types).HasMaxLength(2000);

        builder.HasOne(x => x.Category)
               .WithMany(c => c.Services)
               .HasForeignKey(x => x.CategoryId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}