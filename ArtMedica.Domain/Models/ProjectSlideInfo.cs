using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ArtMedica.Domain.Models;

[Table("ProjectSlideInfos")]
public class ProjectSlideInfo
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Display(Name = "Описание")]
    [MaxLength(2000)]
    public string Description { get; set; }

    [Display(Name = "Ссылка на изображение")]
    [Required]
    public Guid ImgUrlId { get; set; }

    [ForeignKey(nameof(ImgUrlId))]
    public MediaFile ImgUrl { get; set; }

    [Display(Name = "Дата")]
    [Required]
    public DateTime Date { get; set; }

    [Display(Name = "Проект")]
    [Required]
    public Guid ProjectId { get; set; }

    [ForeignKey(nameof(ProjectId))]
    public Project Project { get; set; }

    public override string ToString()
    {
        return Project.Title;
    }
}