using ArtMedica.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ArtMedica.Infrastructure.Configurations;

public class SpecialistConfiguration : IEntityTypeConfiguration<Specialist>
{
    public void Configure(EntityTypeBuilder<Specialist> builder)
    {
        builder.ToTable("Specialists");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.FirstName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(s => s.MidName)
            .HasMaxLength(100);

        builder.Property(s => s.LastName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(s => s.Department)
            .HasMaxLength(200);

        builder.Property(s => s.Description)
            .HasMaxLength(5000);

        builder.Property(s => s.ImgUrl)
            .HasMaxLength(1000);

        builder.Property(s => s.StartDate)
            .IsRequired();

        builder.Property(s => s.OperationsCount)
            .IsRequired();

        builder.Property(s => s.PositiveReviewsPercentage)
            .IsRequired();

        // ICollection<Qualification> — value object
        builder.OwnsMany(s => s.Qualifications, q =>
        {
            q.ToTable("SpecialistQualifications");
            q.WithOwner(x => x.Specialist).HasForeignKey("SpecialistId");
            q.Property(x => x.Title).IsRequired().HasMaxLength(300);
            q.Property(x => x.Level).HasMaxLength(100);
            q.Property(x => x.Organization).HasMaxLength(300);
            q.Property(x => x.IssuanceDate).IsRequired();
        });

        // ICollection<Accreditation> — value object
        builder.OwnsMany(s => s.Accreditations, a =>
        {
            a.ToTable("SpecialistAccreditations");
            a.WithOwner(x => x.Specialist).HasForeignKey("SpecialistId");
            a.Property(x => x.DocumentType).IsRequired().HasMaxLength(200);
            a.Property(x => x.Specialisation).HasMaxLength(300);
            a.Property(x => x.JobTitle).HasMaxLength(200);
            a.Property(x => x.IssuanceDate).IsRequired();
            a.Property(x => x.ValidityPeriod).IsRequired();
            a.Property(x => x.Description).HasMaxLength(2000);
        });

        // 1:N — OperationResult
        builder.HasMany(s => s.operationResults)
            .WithOne(o => o.Specialist)
            .HasForeignKey("SpecialistId")
            .OnDelete(DeleteBehavior.Cascade);

        builder.PrimitiveCollection(s => s.ActivityArea);
    }
}