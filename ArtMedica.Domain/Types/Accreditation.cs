using System.ComponentModel.DataAnnotations.Schema;

namespace ArtMedica.Domain.Models;

public class Accreditation
{
    public string DocumentType { get; set; }
    public string Specialisation { get; set; }
    public string JobTitle { get; set; }
    public DateTime IssuanceDate { get; set; }
    public DateTime ValidityPeriod { get; set; }
    public string Description { get; set; }
    public Specialist Specialist { get; set; }
}