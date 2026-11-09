using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ArtMedica.Domain.Models;

[Table("Faqs")]
public class Faq
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required, MaxLength(500)]
    public string Question { get; set; }

    [Required, MaxLength(2000)]
    public string Answer { get; set; }

    public override string ToString()
    {
        return Question;
    }
}