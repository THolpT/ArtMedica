using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ArtMedica.Domain.Models;

[Table("Accreditations")]
public class Accreditation
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Display(Name = "Тип документа")]
    [Required, MaxLength(100)]
    public string DocumentType { get; set; }

    [Display(Name = "Специализация")]
    [Required, MaxLength(200)]
    public string Specialisation { get; set; }

    [Display(Name = "Должность")]
    [Required, MaxLength(200)]
    public string JobTitle { get; set; }

    [Display(Name = "Дата выдачи")]
    [Required]
    public DateTime IssuanceDate { get; set; }

    [Display(Name = "Срок действия")]
    [Required]
    public DateTime ValidityPeriod { get; set; }

    [Display(Name = "Описание")]
    [MaxLength(1000)]
    public string Description { get; set; }

    [Display(Name = "Специалист")]
    [Required]
    public Guid SpecialistId { get; set; }

    [ForeignKey(nameof(SpecialistId))]
    public Specialist Specialist { get; set; } = null!;

    public override string ToString()
    {
        return $"{DocumentType} — {Specialisation}";
    }
}