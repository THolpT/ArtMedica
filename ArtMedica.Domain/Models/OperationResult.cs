using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ArtMedica.Domain.Models;

public class OperationResult
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; } = Guid.NewGuid();
    public byte[] ImgUrl { get; set; }

    public Guid OperationId { get; set; }

    [ForeignKey(nameof(OperationId))]
    public Service Operation { get; set; } = null!;
    public Guid SpecialistId { get; set; }

    [ForeignKey(nameof(SpecialistId))]
    public Specialist Specialist { get; set; } = null!;

    public override string ToString()
    {
        return Operation.TechnicalTitle;
    }
}