using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ArtMedica.Domain.Models;

public class Case
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; } = Guid.NewGuid();
    public string ClientName { get; set; }
    public string Problem { get; set; }
    public string Solution { get; set; }
    public DateTime Date { get; set; }
    public int Review { get; set; }

    public override string ToString()
    {
        return ClientName;
    }
}