using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ArtMedica.Domain.Models;

[Table("Services")]
public class Service
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required, MaxLength(50)]
    public string NomenclatureCode { get; set; }

    [Required, MaxLength(50)]
    public string MISCode { get; set; }

    [Range(0, int.MaxValue)]
    public int Duration { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; set; }

    [Required, MaxLength(200)]
    public string DisplayTitle { get; set; }

    [Required, MaxLength(200)]
    public string TechnicalTitle { get; set; }

    [MaxLength(2000)]
    public string Description { get; set; }

    [MaxLength(2000)]
    public string Preparation { get; set; }

    [DataType("Markdown")]
    [MaxLength(2000)]
    public string Types { get; set; }

    [Required]
    public Guid CategoryId { get; set; }

    [ForeignKey(nameof(CategoryId))]
    public ServiceCategory Category { get; set; }

    public override string ToString()
    {
        return TechnicalTitle;
    }
}