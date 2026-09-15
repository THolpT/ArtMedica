using ArtMedica.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ArtMedica.Infrastructure.Configurations;

public class OperationResultConfiguration : IEntityTypeConfiguration<OperationResult>
{
    public void Configure(EntityTypeBuilder<OperationResult> builder)
    {
        builder.ToTable("OperationResults");

        builder.HasKey(o => o.Id);

        builder.Property(o => o.ImgUrl)
            .HasMaxLength(1000);

        builder.HasOne(o => o.Operation)
            .WithMany()
            .HasForeignKey("OperationId")
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(o => o.Specialist)
            .WithMany(s => s.operationResults)
            .HasForeignKey("SpecialistId")
            .OnDelete(DeleteBehavior.Cascade);
    }
}