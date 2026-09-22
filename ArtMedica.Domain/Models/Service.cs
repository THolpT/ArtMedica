using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ArtMedica.Domain.Models;

public class Service
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; } = Guid.NewGuid();
    public string NomenclatureCode { get; set; }
    public string MISCode { get; set; }
    public int Duration { get; set; }
    public decimal Price { get; set; }
    public string DisplayTitle { get; set; }
    public string TechnicalTitle { get; set; }
    public string Description { get; set; }
    public string Preparation { get; set; }

    [DataType("Markdown")]
    public string Types { get; set; }
    public Guid CategoryId { get; set; }

    [ForeignKey(nameof(CategoryId))]
    public ServiceCategory Category { get; set; }

    public override string ToString()
    {
        return TechnicalTitle;
    }
}