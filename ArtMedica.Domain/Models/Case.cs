namespace ArtMedica.Domain.Models;

public class Case
{
    public Guid Id {get;set;}
    public string ClientName {get;set;}
    public string Problem {get;set;}
    public string Solution {get;set;}
    public DateTime Date {get;set;}
    public int Review {get;set;}
}