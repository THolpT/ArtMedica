using System.Globalization;
using ArtMedica.WebApi.Abstractions;
using ArtMedica.WebApi.Configuration;
using Microsoft.EntityFrameworkCore;

public sealed class AdminCrudService<TDbContext> : IAdminCrudService
    where TDbContext : DbContext
{
    private readonly TDbContext _db;

    public AdminCrudService(TDbContext db)
    {
        _db = db;
    }

    public async Task<IReadOnlyList<object>> GetListAsync(
        IEntityDefinition definition,
        CancellationToken cancellationToken = default)
    {
        var method = GetType()
            .GetMethod(
                nameof(GetListTypedAsync),
                System.Reflection.BindingFlags.Instance |
                System.Reflection.BindingFlags.NonPublic)!;

        var generic = method.MakeGenericMethod(
            definition.EntityType);

        return await (Task<IReadOnlyList<object>>)generic.Invoke(
            this,
            [
                definition,
                cancellationToken
            ])!;
    }

    private async Task<IReadOnlyList<object>> GetListTypedAsync<TEntity>(
        IEntityDefinition definition,
        CancellationToken cancellationToken)
        where TEntity : class
    {
        var entities = await _db
            .Set<TEntity>()
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return entities
            .Cast<object>()
            .ToList();
    }


    public async Task<object?> GetAsync(
        IEntityDefinition definition,
        string id,
        CancellationToken cancellationToken = default)
    {
        var method = GetType()
            .GetMethod(
                nameof(GetTypedAsync),
                System.Reflection.BindingFlags.Instance |
                System.Reflection.BindingFlags.NonPublic)!;

        var generic = method.MakeGenericMethod(
            definition.EntityType);

        return await (Task<object?>)generic.Invoke(
            this,
            [
                definition,
                id,
                cancellationToken
            ])!;
    }

    private async Task<object?> GetTypedAsync<TEntity>(
        IEntityDefinition definition,
        string id,
        CancellationToken cancellationToken)
        where TEntity : class
    {
        var entityType = _db.Model.FindEntityType(typeof(TEntity));

        if (entityType == null)
            throw new InvalidOperationException(
                $"Entity {typeof(TEntity).Name} is not registered in DbContext.");

        var primaryKey = entityType.FindPrimaryKey();

        if (primaryKey == null)
            throw new InvalidOperationException(
                $"Entity {typeof(TEntity).Name} does not have a primary key.");

        if (primaryKey.Properties.Count != 1)
            throw new InvalidOperationException(
                $"Entity {typeof(TEntity).Name} must have a single primary key.");

        var keyProperty = primaryKey.Properties[0];

        object convertedId;

        try
        {
            convertedId = ConvertValue(
                id,
                keyProperty.ClrType);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException(
                $"Cannot convert id '{id}' to {keyProperty.ClrType.Name}.",
                ex);
        }

        return await _db
            .Set<TEntity>()
            .FindAsync(
                [convertedId],
                cancellationToken);
    }


    public async Task<object> CreateAsync(
        IEntityDefinition definition,
        IReadOnlyDictionary<string, object?> values,
        HttpContext httpContext,
        CancellationToken cancellationToken = default)
    {
        var method = GetType()
            .GetMethod(
                nameof(CreateTypedAsync),
                System.Reflection.BindingFlags.Instance |
                System.Reflection.BindingFlags.NonPublic)!;

        var generic = method.MakeGenericMethod(
            definition.EntityType);

        return await (Task<object>)generic.Invoke(
            this,
            [
                definition,
                values,
                httpContext,
                cancellationToken
            ])!;
    }

    private async Task<object> CreateTypedAsync<TEntity>(
        IEntityDefinition definition,
        IReadOnlyDictionary<string, object?> values,
        HttpContext httpContext,
        CancellationToken cancellationToken)
        where TEntity : class, new()
    {
        var typedDefinition =
            (EntityDefinition<TEntity>)definition;

        var entity = new TEntity();

        foreach (var field in typedDefinition.Fields)
        {
            if (field.ReadOnly)
                continue;

            if (!values.TryGetValue(
                    field.Name,
                    out var value))
                continue;

            field.SetValue(entity, value);
        }

        var context = new AdminCreateContext<TEntity>
        {
            Entity = entity,
            Services = httpContext.RequestServices,
            HttpContext = httpContext,
            Values = values
        };

        await typedDefinition.ExecuteCreateAsync(context);

        _db.Set<TEntity>().Add(entity);

        await _db.SaveChangesAsync(cancellationToken);

        return entity;
    }


    public async Task UpdateAsync(
        IEntityDefinition definition,
        object entity,
        IReadOnlyDictionary<string, object?> values,
        HttpContext httpContext,
        CancellationToken cancellationToken = default)
    {
        var method = GetType()
            .GetMethod(
                nameof(UpdateTypedAsync),
                System.Reflection.BindingFlags.Instance |
                System.Reflection.BindingFlags.NonPublic)!;

        var generic = method.MakeGenericMethod(
            definition.EntityType);

        await (Task)generic.Invoke(
            this,
            [
                definition,
                entity,
                values,
                httpContext,
                cancellationToken
            ])!;
    }

    private async Task UpdateTypedAsync<TEntity>(
        IEntityDefinition definition,
        object entity,
        IReadOnlyDictionary<string, object?> values,
        HttpContext httpContext,
        CancellationToken cancellationToken)
        where TEntity : class, new()
    {
        var typedDefinition =
            (EntityDefinition<TEntity>)definition;

        var typedEntity = entity as TEntity;

        if (typedEntity == null)
        {
            throw new ArgumentException(
                $"Entity must be of type {typeof(TEntity).Name}.");
        }

        foreach (var field in typedDefinition.Fields)
        {
            if (field.ReadOnly)
                continue;

            if (!values.TryGetValue(
                    field.Name,
                    out var value))
                continue;

            field.SetValue(typedEntity, value);
        }

        var context = new AdminUpdateContext<TEntity>
        {
            Entity = typedEntity,
            Services = httpContext.RequestServices,
            HttpContext = httpContext,
            Values = values
        };

        await typedDefinition.ExecuteUpdateAsync(context);

        _db.Set<TEntity>().Update(typedEntity);

        await _db.SaveChangesAsync(cancellationToken);
    }


private static object? ConvertValue(
    string? value,
    Type targetType)
{
    var underlyingType =
        Nullable.GetUnderlyingType(targetType);

    if (string.IsNullOrWhiteSpace(value))
    {
        if (underlyingType != null ||
            !targetType.IsValueType)
        {
            return null;
        }

        return Activator.CreateInstance(targetType);
    }

    if (underlyingType != null)
        targetType = underlyingType;

    if (targetType == typeof(string))
        return value;

    if (targetType == typeof(Guid))
        return Guid.Parse(value);

    if (targetType == typeof(DateTime))
    {
        var dateTime = DateTime.Parse(
            value,
            CultureInfo.InvariantCulture);

        return DateTime.SpecifyKind(
            dateTime,
            DateTimeKind.Unspecified);
    }

    if (targetType == typeof(DateOnly))
    {
        return DateOnly.ParseExact(
            value,
            "yyyy-MM-dd",
            CultureInfo.InvariantCulture);
    }

    if (targetType == typeof(TimeOnly))
    {
        return TimeOnly.Parse(
            value,
            CultureInfo.InvariantCulture);
    }

    if (targetType.IsEnum)
    {
        return Enum.Parse(
            targetType,
            value,
            ignoreCase: true);
    }

    return Convert.ChangeType(
        value,
        targetType,
        CultureInfo.InvariantCulture);
}


    public async Task DeleteAsync(
    IEntityDefinition definition,
    string id,
    CancellationToken cancellationToken = default)
{
    var entity = await GetAsync(
        definition,
        id,
        cancellationToken);

    if (entity is null)
        throw new KeyNotFoundException(
            $"Запись с ID '{id}' не найдена.");

    _db.Remove(entity);

    await _db.SaveChangesAsync(cancellationToken);
}
}