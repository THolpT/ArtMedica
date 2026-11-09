using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ArtMedica.Domain.Models;

[Table("ProjectPartners")]
public class ProjectPartner
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Display(Name = "Название")]
    [Required, MaxLength(200)]
    public string Title { get; set; }

    [Display(Name = "Описание")]
    [MaxLength(1000)]
    public string Description { get; set; }

    [Display(Name = "Проект")]
    [Required]
    public Guid ProjectId { get; set; }

    [ForeignKey(nameof(ProjectId))]
    public Project Project { get; set; } = null!;

    public override string ToString()
    {
        return Title;
    }
}