using ArtMedica.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ArtMedica.Application.Configurations;

public class OperationResultConfiguration : IEntityTypeConfiguration<OperationResult>
{
    public void Configure(EntityTypeBuilder<OperationResult> builder)
    {
        builder.ToTable("OperationResults");
        builder.HasKey(x => x.Id);

        builder.HasOne(x => x.ImgUrl)
               .WithMany()
               .HasForeignKey(x => x.ImgUrlId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Operation)
               .WithMany()
               .HasForeignKey(x => x.OperationId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Specialist)
               .WithMany(s => s.operationResults)
               .HasForeignKey(x => x.SpecialistId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}