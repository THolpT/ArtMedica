using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ArtMedica.Domain.Models;

[Table("ServiceSubcategories")]
public class ServiceSubcategory
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required, MaxLength(200)]
    public string Title { get; set; }

    [Required, MaxLength(1000)]
    public string? Description { get; set; }

    [Display(Name = "Ссылка на изображение")]
    [Required]
    public Guid ImgUrlId { get; set; }

    [ForeignKey(nameof(ImgUrlId))]
    public MediaFile ImgUrl { get; set; }

    [Required]
    public Guid CategoryId { get; set; }

    [ForeignKey(nameof(CategoryId))]
    public ServiceCategory Category { get; set; }

    [InverseProperty(nameof(Service.Category))]
    public ICollection<Service> Services { get; set; } = new List<Service>();

    public override string ToString()
    {
        return Title;
    }
}