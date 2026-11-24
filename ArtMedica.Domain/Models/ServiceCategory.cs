using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ArtMedica.Domain.Models;

[Table("ServiceCategories")]
public class ServiceCategory
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required, MaxLength(200)]
    public string Title { get; set; }

    [Display(Name = "Ссылка на изображение")]
    [Required]
    public Guid ImgUrlId { get; set; }

    [ForeignKey(nameof(ImgUrlId))]
    public MediaFile ImgUrl { get; set; }

    [InverseProperty(nameof(ServiceSubcategory.Category))]
    public ICollection<ServiceSubcategory> ServiceSubcategories { get; set; } = new List<ServiceSubcategory>();

    public override string ToString()
    {
        return Title;
    }
}