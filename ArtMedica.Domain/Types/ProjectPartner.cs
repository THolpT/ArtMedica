namespace ArtMedica.Domain.Models;

public class ProjectPartner
{
    public string Title { get; set; }
    public string Description { get; set; }
    public Project Project { get; set; }
}