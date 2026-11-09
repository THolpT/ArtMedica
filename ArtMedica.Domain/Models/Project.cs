using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ArtMedica.Domain.Models;

[Table("Projects")]
public class Project
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required, MaxLength(200)]
    public string Title { get; set; }

    [Required]
    public Guid MainImgUrlId { get; set; }

    [ForeignKey(nameof(MainImgUrlId))]
    public MediaFile MainImgUrl { get; set; }

    [MaxLength(2000)]
    public string FirstDescription { get; set; }

    [MaxLength(2000)]
    public string SecondDescription { get; set; }

    [Range(0, int.MaxValue)]
    public int SeasonCount { get; set; }

    [Range(0, int.MaxValue)]
    public int MemberCount { get; set; }

    [Range(0, int.MaxValue)]
    public int ExpertCount { get; set; }

    [Range(0, int.MaxValue)]
    public int PartnerCount { get; set; }

    public ProjectSlideInfo SlideInfo { get; set; }

    [InverseProperty(nameof(ProjectFormat.Project))]
    public ICollection<ProjectFormat> Formats { get; set; } = new List<ProjectFormat>();

    public string[] Stages { get; set; } = [];

    public DateTime Date {get;set;}

    public ICollection<MediaFile> Images { get; set; } = new List<MediaFile>();

    [InverseProperty(nameof(ProjectPartner.Project))]
    public ICollection<ProjectPartner> Partners { get; set; } = new List<ProjectPartner>();

    public override string ToString()
    {
        return Title;
    }
}