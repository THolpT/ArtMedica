namespace ArtMedica.Domain.Models;

public class Specialist
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string MidName { get; set; }
    public string LastName { get; set; }
    public string Department { get; set; }
    public string Description {get;set;}
    public string ImgUrl {get;set;}
    public DateTime StartDate {get;set;}
    public int OperationsCount {get;set;}
    public byte PositiveReviewsPercentage {get;set;}
    public ICollection<string> ActivityArea {get;set;}
    public ICollection<Qualification> Qualifications {get;set;}
    public ICollection<Accreditation> Accreditations {get;set;}
    public ICollection<OperationResult> operationResults {get;set;}
} 