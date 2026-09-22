using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ArtMedica.Domain.Models;

public class New
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Title { get; set; }
    public byte[] ImgUrl { get; set; }
    public string Description { get; set; }

    public override string ToString()
    {
        return Title;
    }
}