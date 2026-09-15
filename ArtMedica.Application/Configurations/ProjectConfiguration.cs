using ArtMedica.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ArtMedica.Application.Configurations;

public class ProjectConfiguration : IEntityTypeConfiguration<Project>
{
    public void Configure(EntityTypeBuilder<Project> builder)
    {
        builder.ToTable("Projects");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Title)
            .IsRequired()
            .HasMaxLength(300);

        builder.Property(p => p.MainImgUrl)
            .HasMaxLength(1000);

        builder.Property(p => p.FirstDescription)
            .HasMaxLength(5000);

        builder.Property(p => p.SecondDescription)
            .HasMaxLength(5000);

        builder.Property(p => p.SeasonCount)
            .IsRequired();

        builder.Property(p => p.MemberCount)
            .IsRequired();

        builder.Property(p => p.ExpertCount)
            .IsRequired();

        builder.Property(p => p.PartnerCount)
            .IsRequired();

        // 1:1 — ProjectSlideInfo
        builder.HasOne(p => p.SlideInfo)
            .WithOne(s => s.Project)
            .HasForeignKey<ProjectSlideInfo>(s => s.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);

        // 1:N — ProjectFormat (owned)
        builder.OwnsMany(p => p.Formats, f =>
        {
            f.ToTable("ProjectFormats");
            f.WithOwner(x => x.Project).HasForeignKey("ProjectId");
            f.Property(x => x.Title).IsRequired().HasMaxLength(200);
            f.Property(x => x.Description).HasMaxLength(2000);
            f.Property(x => x.ImgUrl).HasMaxLength(1000);
        });

        // 1:N — ProjectPartner (owned)
        builder.OwnsMany(p => p.Partners, pt =>
        {
            pt.ToTable("ProjectPartners");
            pt.WithOwner(x => x.Project).HasForeignKey("ProjectId");
            pt.Property(x => x.Title).IsRequired().HasMaxLength(200);
            pt.Property(x => x.Description).HasMaxLength(2000);
        });

        builder.PrimitiveCollection(p => p.Stages);
        builder.PrimitiveCollection(p => p.Images);
    }
}