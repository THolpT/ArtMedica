namespace ArtMedica.Domain.Models;

public class ProjectSlideInfo
{
    public Guid Id { get; set; }
    public string Description { get; set; }
    public string ImgUrl {get;set;}
    public DateTime Date { get; set; }
    public Project Project { get; set; }
}