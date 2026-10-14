using ArtMedica.WebApi.Abstractions;

public sealed class AdminRegistry
    : IAdminRegistry
{
    private readonly Dictionary<
        string,
        IEntityDefinition> _entities
        = new(StringComparer.OrdinalIgnoreCase);

    public IReadOnlyCollection<IEntityDefinition> Entities =>
        _entities.Values;

    public void Register(
        IEntityDefinition definition)
    {
        if (!_entities.TryAdd(
                definition.Route,
                definition))
        {
            throw new InvalidOperationException(
                $"Admin entity '{definition.Route}' is already registered.");
        }
    }

    public IEntityDefinition Get(
        string route)
    {
        if (!_entities.TryGetValue(
                route,
                out var definition))
        {
            throw new KeyNotFoundException(
                $"Admin entity '{route}' was not found.");
        }

        return definition;
    }

    public IEntityDefinition Get(Type type)
    {
        return _entities.Values.First(x =>
            x.EntityType == type);
    }
}