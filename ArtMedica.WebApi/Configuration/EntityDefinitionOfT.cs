namespace ArtMedica.WebApi.Configuration;

public sealed class EntityDefinition<TEntity>
    : EntityDefinition
    where TEntity : class, new()
{
    public Func<
        AdminCreateContext<TEntity>,
        Task>? OnCreating { get; set; }

    public Func<
        AdminCreateContext<TEntity>,
        Task<TEntity>>? Create { get; set; }

    public Func<
        AdminUpdateContext<TEntity>,
        Task>? OnUpdating { get; set; }

    public Func<
        AdminUpdateContext<TEntity>,
        Task>? Update { get; set; }

    public EntityDefinition()
    {
        EntityType = typeof(TEntity);
    }

    public void AddField(FieldDefinition field)
    {
        var fields = Fields.ToList();
        fields.Add(field);

        Fields = fields;
    }

    public async Task<TEntity> ExecuteCreateAsync(
        AdminCreateContext<TEntity> context)
    {
        if (OnCreating is not null)
            await OnCreating(context);

        if (Create is not null)
            return await Create(context);

        throw new InvalidOperationException(
            $"Create is not configured for {typeof(TEntity).Name}");
    }

    public async Task ExecuteUpdateAsync(
        AdminUpdateContext<TEntity> context)
    {
        if (OnUpdating is not null)
            await OnUpdating(context);

        if (Update is not null)
            await Update(context);
    }

    public override Task<object> CreateAsync(
        AdminCreateContext<object> context)
    {
        throw new NotImplementedException();
    }

    public override Task UpdateAsync(
        AdminUpdateContext<object> context,
        object entity)
    {
        throw new NotImplementedException();
    }
}