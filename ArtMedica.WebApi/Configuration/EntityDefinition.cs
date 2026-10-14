using ArtMedica.WebApi.Abstractions;
using ArtMedica.WebApi.Configuration;

public abstract class EntityDefinition : IEntityDefinition
{
    public string Name { get; set; } = default!;

    public string Label { get; set; } = default!;

    public string Route { get; set; } = default!;

    public Type EntityType { get; set; } = default!;

    public IReadOnlyList<FieldDefinition> Fields { get; internal set; }
        = [];

    public abstract Task<object> CreateAsync(
        AdminCreateContext<object> context);

    public abstract Task UpdateAsync(
        AdminUpdateContext<object> context,
        object entity);
}