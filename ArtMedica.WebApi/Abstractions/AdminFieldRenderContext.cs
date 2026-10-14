using ArtMedica.WebApi.Configuration;

namespace ArtMedica.WebApi.Abstractions;
public sealed class AdminFieldRenderContext
{
    public required FieldDefinition Definition { get; init; }

    public IServiceProvider Services { get; init; } = default!;

    public object? Value { get; init; }

    public string? Error { get; init; }

    public bool Disabled =>
        Definition.ReadOnly;

    public string InputName =>
        Definition.Name;
}