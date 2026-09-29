using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ArtMedica.Domain.Models;

[Table("OperationResults")]
public class OperationResult
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    public Guid ImgUrlId { get; set; }

    [ForeignKey(nameof(ImgUrlId))]
    public MediaFile ImgUrl { get; set; } = null!;

    [Required]
    public Guid OperationId { get; set; }

    [ForeignKey(nameof(OperationId))]
    public Service Operation { get; set; } = null!;

    [Required]
    public Guid SpecialistId { get; set; }

    [ForeignKey(nameof(SpecialistId))]
    public Specialist Specialist { get; set; } = null!;

    public override string ToString()
    {
        return Operation.TechnicalTitle;
    }
}