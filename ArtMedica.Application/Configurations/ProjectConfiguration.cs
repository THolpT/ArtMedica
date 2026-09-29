using ArtMedica.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ArtMedica.Infrastructure.Configurations;

public class ProjectConfiguration : IEntityTypeConfiguration<Project>
{
    public void Configure(EntityTypeBuilder<Project> builder)
    {
        builder.ToTable("Projects");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Title)
               .IsRequired()
               .HasMaxLength(200);

        builder.Property(x => x.FirstDescription).HasMaxLength(2000);
        builder.Property(x => x.SecondDescription).HasMaxLength(2000);

        // --- Примитивная коллекция Stages ---
        // EF Core 8+ поддерживает нативно. Для EF Core 7- используйте value converter (см. ниже).
        builder.PrimitiveCollection(x => x.Stages)
               .HasColumnName("Stages");

        // Основная картинка
        builder.HasOne(x => x.MainImgUrl)
               .WithMany()
               .HasForeignKey(x => x.MainImgUrlId)
               .OnDelete(DeleteBehavior.Restrict);

        // Один-к-одному с ProjectSlideInfo, ведущая сторона — ProjectSlideInfo
        builder.HasOne(x => x.SlideInfo)
               .WithOne(s => s.Project)
               .HasForeignKey<ProjectSlideInfo>(s => s.ProjectId)
               .OnDelete(DeleteBehavior.Cascade);

        // Форматы проекта
        builder.HasMany(x => x.Formats)
               .WithOne(f => f.Project)
               .HasForeignKey(f => f.ProjectId)
               .OnDelete(DeleteBehavior.Cascade);

        // Партнёры проекта
        builder.HasMany(x => x.Partners)
               .WithOne(p => p.Project)
               .HasForeignKey(p => p.ProjectId)
               .OnDelete(DeleteBehavior.Cascade);

        // Изображения проекта (many-to-many без явной сущности-связки)
        builder.HasMany(x => x.Images)
               .WithMany()
               .UsingEntity<Dictionary<string, object>>(
                   "ProjectImages",
                   j => j.HasOne<MediaFile>().WithMany().HasForeignKey("FileId").OnDelete(DeleteBehavior.Cascade),
                   j => j.HasOne<Project>().WithMany().HasForeignKey("ProjectId").OnDelete(DeleteBehavior.Cascade),
                   j =>
                   {
                       j.ToTable("ProjectImages");
                       j.HasKey("ProjectId", "FileId");
                   });
    }
}