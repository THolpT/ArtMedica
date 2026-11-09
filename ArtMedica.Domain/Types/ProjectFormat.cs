using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ArtMedica.Domain.Models;

[Table("ProjectFormats")]
public class ProjectFormat
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

    [Display(Name = "Изображение")]
    [Required]
    public Guid ImgId { get; set; }

    [ForeignKey(nameof(ImgId))]
    public MediaFile Img { get; set; } = null!;

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