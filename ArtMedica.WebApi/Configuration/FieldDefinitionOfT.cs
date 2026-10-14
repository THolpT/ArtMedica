using ArtMedica.WebApi.Configuration;

namespace ArtMedica.WebApi.Configuration;
public sealed class FieldDefinition<TEntity, TValue>
    : FieldDefinition
{
    private readonly Func<TEntity, TValue?> _getter;

    private readonly Action<TEntity, TValue?>? _setter;

    public FieldDefinition(
        string name,
        Func<TEntity, TValue?> getter,
        Action<TEntity, TValue?>? setter = null)
    {
        Name = name;
        _getter = getter;
        _setter = setter;
    }

    public override Type ValueType =>
        typeof(TValue);

    public override object? GetValue(object entity)
    {
        return _getter((TEntity)entity);
    }

    public override void SetValue(
        object entity,
        object? value)
    {
        if (_setter is null)
            return;

        var converted = ConvertValue(value);

        _setter(
            (TEntity)entity,
            (TValue?)converted);
    }

    private static object? ConvertValue(object? value)
    {
        if (value is null)
            return default;

        if (value is TValue typed)
            return typed;

        var targetType =
            Nullable.GetUnderlyingType(typeof(TValue))
            ?? typeof(TValue);

        if (targetType.IsEnum)
        {
            return Enum.Parse(
                targetType,
                value.ToString()!,
                ignoreCase: true);
        }

        if (targetType == typeof(Guid))
        {
            return Guid.Parse(value.ToString()!);
        }

        if (targetType == typeof(string))
        {
            return value.ToString();
        }

        return Convert.ChangeType(
            value,
            targetType);
    }
}