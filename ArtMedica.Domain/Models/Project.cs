namespace ArtMedica.Domain.Models;

public class Project
{
    public Guid Id {get;set;}
    public string Title {get;set;}
    public string MainImgUrl {get;set;}
    public string FirstDescription {get;set;}
    public string SecondDescription {get;set;}
    public int SeasonCount {get;set;}
    public int MemberCount {get;set;}
    public int ExpertCount {get;set;}
    public int PartnerCount {get;set;}
    public ProjectSlideInfo SlideInfo {get;set;}
    public ICollection<ProjectFormat> Formats {get;set;}
    public ICollection<string> Stages {get;set;}
    public ICollection<string> Images {get;set;}
    public ICollection<ProjectPartner> Partners {get;set;}
}