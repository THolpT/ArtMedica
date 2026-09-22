using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ArtMedica.Domain.Models;

public class Specialist
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; } = Guid.NewGuid();
    public string FirstName { get; set; }
    public string MidName { get; set; }
    public string LastName { get; set; }
    public string Department { get; set; }
    public string Description { get; set; }
    public byte[] ImgUrl { get; set; }
    public DateTime StartDate { get; set; }
    public int OperationsCount { get; set; }
    public byte PositiveReviewsPercentage { get; set; }
    public ICollection<string> ActivityArea { get; set; }
    public ICollection<Qualification> Qualifications { get; set; }
    public ICollection<Accreditation> Accreditations { get; set; }
    public ICollection<OperationResult> operationResults { get; set; }

    public override string ToString()
    {
        return LastName + " " + FirstName + " " + MidName;
    }
}