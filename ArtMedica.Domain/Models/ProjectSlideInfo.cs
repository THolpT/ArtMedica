using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ArtMedica.Domain.Models;

public class ProjectSlideInfo
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Display(Name = "Описание")]
    public string Description { get; set; }

    [Display(Name = "Ссылка на изображение")]
    public byte[] ImgUrl { get; set; }

    [Display(Name = "Дата")]
    public DateTime Date { get; set; }

    [Display(Name = "Проект")]
    public Guid ProjectId { get; set; }

    [ForeignKey(nameof(ProjectId))]
    public Project Project { get; set; }

    public override string ToString()
    {
        return Project.Title;
    }
}