namespace ArtMedica.Domain.Models;

public class Qualification
{
    public string Title { get; set; }
    public string Level { get; set; }
    public string Organization { get; set; }
    public DateTime IssuanceDate { get; set; }
    public Specialist Specialist { get; set; }
}