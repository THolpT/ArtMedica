using ArtMedica.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ArtMedica.Infrastructure.Configurations;

public class ProjectSlideInfoConfiguration : IEntityTypeConfiguration<ProjectSlideInfo>
{
    public void Configure(EntityTypeBuilder<ProjectSlideInfo> builder)
    {
        builder.ToTable("ProjectSlideInfos");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.Description)
            .HasMaxLength(2000);

        builder.Property(s => s.ImgUrl)
            .HasMaxLength(1000);

        builder.Property(s => s.Date)
            .IsRequired();

        // Связь настроена в ProjectConfiguration
    }
}