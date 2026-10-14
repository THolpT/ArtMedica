
using ArtMedica.WebApi.Abstractions;
using Microsoft.AspNetCore.Html;
using Microsoft.Extensions.DependencyInjection;
using System.Globalization;
using System.Reflection;
using System.Text;

public sealed class ReferenceField<TModel> : FieldBase
    where TModel : class
{
    private readonly string _entityRoute;

    public ReferenceField(string entityRoute)
    {
        _entityRoute = entityRoute;
    }

    public override async Task<IHtmlContent> RenderAsync(
        AdminFieldRenderContext context)
    {
        var services = context.Services;

        var registry = services.GetRequiredService<IAdminRegistry>();
        var crudService = services.GetRequiredService<IAdminCrudService>();

        var entityDefinition = registry.Get(_entityRoute);

        if (entityDefinition.EntityType != typeof(TModel))
        {
            throw new InvalidOperationException(
                $"Маршрут '{_entityRoute}' зарегистрирован для " +
                $"типа {entityDefinition.EntityType.Name}, " +
                $"но ожидался {typeof(TModel).Name}.");
        }

        var items = await crudService.GetListAsync(entityDefinition);

        var idProperty = typeof(TModel).GetProperty(
            "Id",
            BindingFlags.Public | BindingFlags.Instance);

        if (idProperty is null)
        {
            throw new InvalidOperationException(
                $"У модели {typeof(TModel).Name} отсутствует публичное свойство Id.");
        }

        var selectedId = Convert.ToString(
            context.Value,
            CultureInfo.InvariantCulture) ?? "";

        var html = new StringBuilder();

        html.Append($"""
            <div class="mb-3">
                <label class="form-label"
                       for="{Encode(context.Definition.Name)}">
                    {Encode(context.Definition.Label)}
                </label>

                <select
                    class="form-select"
                    id="{Encode(context.Definition.Name)}"
                    name="{Encode(context.Definition.Name)}"
                    {(context.Definition.Required ? "required" : "")}
                    {(context.Definition.ReadOnly ? "disabled" : "")}>
            """);

        if (!context.Definition.Required)
        {
            html.Append("""<option value="">Не выбрано</option>""");
        }

        foreach (var item in items)
        {
            if (item is not TModel model)
                continue;

            var id = idProperty.GetValue(model);

            if (id is null)
                continue;

            var idText = Convert.ToString(
                id,
                CultureInfo.InvariantCulture) ?? "";

            var label = model.ToString() ?? idText;
            var selected = idText == selectedId ? "selected" : "";

            html.Append($"""
                <option value="{Encode(idText)}" {selected}>
                    {Encode(label)}
                </option>
                """);
        }

        html.Append("""
                </select>
            </div>
            """);

        return new HtmlString(html.ToString());
    }
}
