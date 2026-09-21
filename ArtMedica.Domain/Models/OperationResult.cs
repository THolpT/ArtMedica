namespace ArtMedica.Domain.Models;

public class OperationResult
{
    public Guid Id { get; set; }
    public string ImgUrl { get; set; } = string.Empty;

    public Guid OperationId { get; set; }
    public Service Operation { get; set; } = null!;

    public Guid SpecialistId { get; set; }
    public Specialist Specialist { get; set; } = null!;
}