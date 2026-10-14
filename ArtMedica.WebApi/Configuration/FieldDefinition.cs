using ArtMedica.WebApi.Abstractions;

namespace ArtMedica.WebApi.Configuration;

public abstract class FieldDefinition
{
    public string Name { get; set; } = default!;

    public string Label { get; set; } = default!;

    public IAdminField Field { get; set; } = default!;

    public bool Required { get; set; }

    public bool ReadOnly { get; set; }

    public string? Placeholder { get; set; }

    public IReadOnlyList<SelectOption> Options { get; set; } = [];

    public abstract Type ValueType { get; }

    public abstract object? GetValue(object entity);

    public abstract void SetValue(object entity, object? value);
}

public record SelectOption(
    string Value,
    string Text);
