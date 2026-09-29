using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ArtMedica.Domain.Models;

[Table("News")]
public class New
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required, MaxLength(200)]
    public string Title { get; set; }

    [Required]
    public Guid ImgUrlId { get; set; }

    [ForeignKey(nameof(ImgUrlId))]
    public MediaFile ImgUrl { get; set; }

    [MaxLength(2000)]
    public string Description { get; set; }

    public override string ToString()
    {
        return Title;
    }
}