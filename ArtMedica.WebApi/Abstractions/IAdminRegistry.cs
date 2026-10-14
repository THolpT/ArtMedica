namespace ArtMedica.WebApi.Abstractions;

public interface IAdminRegistry
{
    IReadOnlyCollection<IEntityDefinition> Entities { get; }

    IEntityDefinition Get(string route);

    IEntityDefinition Get(Type type);
}