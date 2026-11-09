using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ArtMedica.Domain.Models;

[Table("Documents")]
public class Document
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required, MaxLength(255)]
    public string Name { get; set; } = null!;

    [Required]
    public Guid TypeId { get; set; }

    [ForeignKey(nameof(TypeId))]
    public DocumentType Type { get; set; } = null!;

    [Required, MaxLength(500)]
    public string Url { get; set; } = null!;

    public override string ToString() => Name;
}