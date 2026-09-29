using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ArtMedica.Domain.Models;

[Table("Files")]
public class MediaFile
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required, MaxLength(255)]
    public string Name { get; set; }

    [Required, MaxLength(100)]
    public string Type { get; set; }

    [Required, MaxLength(500)]
    public string Url { get; set; }

    public override string ToString()
    {
        return Name;
    }
}