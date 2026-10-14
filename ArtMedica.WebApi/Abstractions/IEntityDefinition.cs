using ArtMedica.WebApi.Configuration;
namespace ArtMedica.WebApi.Abstractions;
public interface IEntityDefinition
{
    string Name { get; }

    string Label { get; }

    string Route { get; }

    Type EntityType { get; }

    IReadOnlyList<FieldDefinition> Fields { get; }

    Task<object> CreateAsync(
        AdminCreateContext<object> context);

    Task UpdateAsync(
        AdminUpdateContext<object> context,
        object entity);
}