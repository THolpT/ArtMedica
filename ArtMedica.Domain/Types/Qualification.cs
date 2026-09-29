using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ArtMedica.Domain.Models;

[Table("Qualifications")]
public class Qualification
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required, MaxLength(200)]
    public string Title { get; set; }

    [Required, MaxLength(100)]
    public string Level { get; set; }

    [Required, MaxLength(200)]
    public string Organization { get; set; }

    [Required]
    public DateTime IssuanceDate { get; set; }

    [Required]
    public Guid SpecialistId { get; set; }

    [ForeignKey(nameof(SpecialistId))]
    public Specialist Specialist { get; set; }
}