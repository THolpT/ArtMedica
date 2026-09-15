using ArtMedica.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ArtMedica.Application.Configurations;

public class NewConfiguration : IEntityTypeConfiguration<New>
{
    public void Configure(EntityTypeBuilder<New> builder)
    {
        builder.ToTable("News");

        builder.HasKey(n => n.Id);

        builder.Property(n => n.Title)
            .IsRequired()
            .HasMaxLength(300);

        builder.Property(n => n.ImgUrl)
            .HasMaxLength(1000);

        builder.Property(n => n.Description)
            .IsRequired()
            .HasMaxLength(5000);
    }
}