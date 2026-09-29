using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ArtMedica.Domain.Models;

[Table("Cases")]
public class Case
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required, MaxLength(200)]
    public string ClientName { get; set; }

    [Required, MaxLength(2000)]
    public string Problem { get; set; }

    [Required, MaxLength(2000)]
    public string Solution { get; set; }

    [Required]
    public DateTime Date { get; set; }

    [Range(0, 5)]
    public int Review { get; set; }

    public override string ToString()
    {
        return ClientName;
    }
}