namespace ArtMedica.Domain.Models;

public class ProjectFormat
{
    public string Title { get; set; }
    public string Description { get; set; }
    public string ImgUrl {get;set;}
    public Project Project { get; set; }
}