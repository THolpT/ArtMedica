using ArtMedica.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ArtMedica.Infrastructure.Configurations;

public class SpecialistConfiguration : IEntityTypeConfiguration<Specialist>
{
    public void Configure(EntityTypeBuilder<Specialist> builder)
    {
        builder.ToTable("Specialists");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.FirstName).IsRequired().HasMaxLength(100);
        builder.Property(x => x.MidName).IsRequired().HasMaxLength(100);
        builder.Property(x => x.LastName).IsRequired().HasMaxLength(100);
        builder.Property(x => x.Department).HasMaxLength(200);
        builder.Property(x => x.Description).HasMaxLength(2000);

        // --- Примитивная коллекция ActivityArea ---
        builder.PrimitiveCollection(x => x.ActivityArea)
               .HasColumnName("ActivityArea");

        // Фото
        builder.HasOne(x => x.Img)
               .WithMany()
               .HasForeignKey(x => x.ImgId)
               .OnDelete(DeleteBehavior.Restrict);

        // Видео
        builder.HasOne(x => x.Video)
               .WithMany()
               .HasForeignKey(x => x.VideoId)
               .OnDelete(DeleteBehavior.Restrict);

        // Квалификации
        builder.HasMany(x => x.Qualifications)
               .WithOne(q => q.Specialist)
               .HasForeignKey(q => q.SpecialistId)
               .OnDelete(DeleteBehavior.Cascade);

        // Аккредитации
        builder.HasMany(x => x.Accreditations)
               .WithOne(a => a.Specialist)
               .HasForeignKey(a => a.SpecialistId)
               .OnDelete(DeleteBehavior.Cascade);

        // Результаты операций
        builder.HasMany(x => x.operationResults)
               .WithOne(r => r.Specialist)
               .HasForeignKey(r => r.SpecialistId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => x.LastName);
    }
}