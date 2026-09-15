using System.ComponentModel.DataAnnotations.Schema;

namespace ArtMedica.Domain.Models;

public class ProjectSlideInfo
{
    public Guid Id { get; set; }
    public string Description { get; set; }
    public string ImgUrl {get;set;}
    public DateTime Date { get; set; }
    public Guid ProjectId { get; set; }

    [ForeignKey(nameof(ProjectId))]
    public Project Project { get; set; }
}