namespace ArtMedica.WebApi.DTOs
{
    public sealed record CaseDto(
        Guid Id,
        string ClientName,
        string Problem,
        string Solution,
        DateTime Date,
        int Review
    );
}
