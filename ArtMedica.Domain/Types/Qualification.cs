using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ArtMedica.Domain.Models;

[Table("Qualifications")]
public class Qualification
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Display(Name = "Название")]
    [Required, MaxLength(200)]
    public string Title { get; set; }

    [Display(Name = "Уровень")]
    [Required, MaxLength(100)]
    public string Level { get; set; }

    [Display(Name = "Организация")]
    [Required, MaxLength(200)]
    public string Organization { get; set; }

    [Display(Name = "Дата выдачи")]
    [Required]
    public DateTime IssuanceDate { get; set; }

    [Display(Name = "Специалист")]
    [Required]
    public Guid SpecialistId { get; set; }

    [ForeignKey(nameof(SpecialistId))]
    public Specialist Specialist { get; set; } = null!;

    public override string ToString()
    {
        return $"{Title} ({Level})";
    }
}