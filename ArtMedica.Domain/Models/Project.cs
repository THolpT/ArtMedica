using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ArtMedica.Domain.Models;

public class Project
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Title { get; set; }
    public byte[] MainImgUrl { get; set; }
    public string FirstDescription { get; set; }
    public string SecondDescription { get; set; }
    public int SeasonCount { get; set; }
    public int MemberCount { get; set; }
    public int ExpertCount { get; set; }
    public int PartnerCount { get; set; }
    public Guid SlideInfoId { get; set; }

    [ForeignKey(nameof(SlideInfoId))]
    public ProjectSlideInfo SlideInfo { get; set; }
    public ICollection<ProjectFormat> Formats { get; set; }
    public ICollection<string> Stages { get; set; }
    public ICollection<string> Images { get; set; }
    public ICollection<ProjectPartner> Partners { get; set; }

    public override string ToString()
    {
        return Title;
    }
}