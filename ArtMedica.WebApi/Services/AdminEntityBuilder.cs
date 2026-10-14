using System.Linq.Expressions;
using System.Reflection;
using ArtMedica.WebApi.Configuration;

public sealed class AdminEntityBuilder<TEntity>
    where TEntity : class, new()
{
    private readonly EntityDefinition<TEntity> _definition;

    public AdminEntityBuilder(
        EntityDefinition<TEntity> definition)
    {
        _definition = definition;
    }

    public string Name
    {
        set => _definition.Name = value;
    }

    public string Label
    {
        set => _definition.Label = value;
    }

    public string Route
    {
        set => _definition.Route = value;
    }

    public AdminFieldBuilder<TEntity, TValue> Field<TValue>(
        string name,
        Func<TEntity, TValue?> getter,
        Action<TEntity, TValue?>? setter = null)
    {
        var field = new FieldDefinition<TEntity, TValue>(
            name,
            getter,
            setter);

        _definition.AddField(field);

        return new AdminFieldBuilder<TEntity, TValue>(field);
    }

    public AdminFieldBuilder<TEntity, TValue> Field<TValue>(
        string name,
        Expression<Func<TEntity, TValue?>> expression)
    {
        var getter = expression.Compile();
        var setter = CreateSetter(expression);

        return Field(
            name,
            getter,
            setter);
    }

    public void OnCreating(
        Func<AdminCreateContext<TEntity>, Task> handler)
    {
        _definition.OnCreating = handler;
    }

    public void Create(
        Func<AdminCreateContext<TEntity>, Task<TEntity>> handler)
    {
        _definition.Create = handler;
    }

    public void OnUpdating(
        Func<AdminUpdateContext<TEntity>, Task> handler)
    {
        _definition.OnUpdating = handler;
    }

    public void Update(
        Func<AdminUpdateContext<TEntity>, Task> handler)
    {
        _definition.Update = handler;
    }

    private static Action<TEntity, TValue?>? CreateSetter<TValue>(
        Expression<Func<TEntity, TValue?>> expression)
    {
        if (expression.Body is not MemberExpression member)
            return null;

        if (member.Member is not PropertyInfo property)
            return null;

        if (!property.CanWrite)
            return null;

        return (entity, value) =>
            property.SetValue(entity, value);
    }
}