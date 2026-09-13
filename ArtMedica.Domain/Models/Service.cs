namespace ArtMedica.Domain.Models;

public class Service
{
    public Guid Id { get; set; }
    public string NomenclatureCode { get; set; }
    public string MISCode { get; set; }
    public int Duration { get; set; }
    public decimal Price { get; set; }
    public string DisplayTitle { get; set; }
    public string TechnicalTitle { get; set; }
    public string Description { get; set; }
    public string Preparation { get; set; }

    public ServiceCategory Category { get; set; }

    // TODO: Виды
}