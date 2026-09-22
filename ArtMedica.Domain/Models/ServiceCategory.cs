using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ArtMedica.Domain.Models;

public class ServiceCategory
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Title { get; set; }
    public ICollection<Service> Services { get; set; } = new List<Service>();

    public override string ToString()
    {
        return Title;
    }
}