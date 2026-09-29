using ArtMedica.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ArtMedica.Infrastructure.Configurations;

public class ProjectSlideInfoConfiguration : IEntityTypeConfiguration<ProjectSlideInfo>
{
    public void Configure(EntityTypeBuilder<ProjectSlideInfo> builder)
    {
        builder.ToTable("ProjectSlideInfos");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Description)
               .HasMaxLength(2000);

        builder.Property(x => x.Date)
               .IsRequired();

        builder.HasOne(x => x.Project)
               .WithOne(p => p.SlideInfo)
               .HasForeignKey<ProjectSlideInfo>(x => x.ProjectId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.ImgUrl)
               .WithMany()
               .HasForeignKey(x => x.ImgUrlId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}