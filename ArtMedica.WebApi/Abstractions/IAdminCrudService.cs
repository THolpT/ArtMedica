using Microsoft.AspNetCore.Http;
namespace ArtMedica.WebApi.Abstractions;
public interface IAdminCrudService
{
    Task<IReadOnlyList<object>> GetListAsync(
        IEntityDefinition definition,
        CancellationToken cancellationToken = default);

    Task<object?> GetAsync(
        IEntityDefinition definition,
        string id,
        CancellationToken cancellationToken = default);

    Task<object> CreateAsync(
        IEntityDefinition definition,
        IReadOnlyDictionary<string, object?> values,
        HttpContext httpContext,
        CancellationToken cancellationToken = default);

    Task UpdateAsync(
        IEntityDefinition definition,
        object entity,
        IReadOnlyDictionary<string, object?> values,
        HttpContext httpContext,
        CancellationToken cancellationToken = default);

    Task DeleteAsync(
    IEntityDefinition definition,
    string id,
    CancellationToken cancellationToken = default);
}