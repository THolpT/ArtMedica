namespace ArtMedica.WebApi.DTOs
{
    public sealed record PromotionDto(
        Guid Id,
        string Title,
        string? Description,
        DateTime Date
    );
}
