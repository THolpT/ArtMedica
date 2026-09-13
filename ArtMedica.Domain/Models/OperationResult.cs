namespace ArtMedica.Domain.Models;

public class OperationResult
{
    public Guid Id {get;set;}
    public string ImgUrl {get;set;}
    public Service Operation {get;set;}
    public Specialist Specialist {get;set;}
}