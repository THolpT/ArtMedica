using ArtMedica.WebApi.Abstractions;
using ArtMedica.WebApi.Configuration;

public sealed class AdminFieldBuilder<TEntity, TValue>
{
    private readonly FieldDefinition<TEntity, TValue>
        _definition;

    public AdminFieldBuilder(
        FieldDefinition<TEntity, TValue> definition)
    {
        _definition = definition;
    }

    public AdminFieldBuilder<TEntity, TValue> Label(
        string label)
    {
        _definition.Label = label;
        return this;
    }

    public AdminFieldBuilder<TEntity, TValue> Field(IAdminField field)
    {
        _definition.Field = field;
        return this;
    }

    public AdminFieldBuilder<TEntity, TValue> Required(
        bool required = true)
    {
        _definition.Required = required;
        return this;
    }

    public AdminFieldBuilder<TEntity, TValue> ReadOnly(
        bool readOnly = true)
    {
        _definition.ReadOnly = readOnly;
        return this;
    }

    public AdminFieldBuilder<TEntity, TValue> Placeholder(
        string placeholder)
    {
        _definition.Placeholder = placeholder;
        return this;
    }

    public AdminFieldBuilder<TEntity, TValue> Use(
        IAdminField field)
    {
        _definition.Field = field;
        return this;
    }

    public AdminFieldBuilder<TEntity, TValue> Options(
        params SelectOption[] options)
    {
        _definition.Options = options;
        return this;
    }
}