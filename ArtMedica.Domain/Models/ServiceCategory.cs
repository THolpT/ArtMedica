namespace ArtMedica.Domain.Models;

public class ServiceCategory
{
    public Guid Id {get;set;}
    public string Title {get;set;}
    public ICollection<Service> Services {get;set;} = new List<Service>();
}