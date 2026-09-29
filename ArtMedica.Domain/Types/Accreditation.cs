using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ArtMedica.Domain.Models;

[Table("Accreditations")]
public class Accreditation
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required, MaxLength(100)]
    public string DocumentType { get; set; }

    [Required, MaxLength(200)]
    public string Specialisation { get; set; }

    [Required, MaxLength(200)]
    public string JobTitle { get; set; }

    [Required]
    public DateTime IssuanceDate { get; set; }

    [Required]
    public DateTime ValidityPeriod { get; set; }

    [MaxLength(1000)]
    public string Description { get; set; }

    [Required]
    public Guid SpecialistId { get; set; }

    [ForeignKey(nameof(SpecialistId))]
    public Specialist Specialist { get; set; }
}