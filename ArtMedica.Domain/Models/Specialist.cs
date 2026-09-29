using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ArtMedica.Domain.Models;

[Table("Specialists")]
public class Specialist
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required, MaxLength(100)]
    public string FirstName { get; set; }

    [Required, MaxLength(100)]
    public string MidName { get; set; }

    [Required, MaxLength(100)]
    public string LastName { get; set; }

    [MaxLength(200)]
    public string Department { get; set; }

    [MaxLength(2000)]
    public string Description { get; set; }

    [Required]
    public Guid ImgId { get; set; }

    [ForeignKey(nameof(ImgId))]
    public MediaFile Img { get; set; }

    [Required]
    public Guid VideoId { get; set; }

    [ForeignKey(nameof(VideoId))]
    public MediaFile Video { get; set; }

    [Required]
    public DateTime StartDate { get; set; }

    [Range(0, int.MaxValue)]
    public int OperationsCount { get; set; }

    [Range(0, 100)]
    public byte PositiveReviewsPercentage { get; set; }

    public ICollection<string> ActivityArea { get; set; } = new List<string>();

    [InverseProperty(nameof(Qualification.Specialist))]
    public ICollection<Qualification> Qualifications { get; set; } = new List<Qualification>();

    [InverseProperty(nameof(Accreditation.Specialist))]
    public ICollection<Accreditation> Accreditations { get; set; } = new List<Accreditation>();

    [InverseProperty(nameof(OperationResult.Specialist))]
    public ICollection<OperationResult> operationResults { get; set; } = new List<OperationResult>();

    public override string ToString()
    {
        return LastName + " " + FirstName + " " + MidName;
    }
}