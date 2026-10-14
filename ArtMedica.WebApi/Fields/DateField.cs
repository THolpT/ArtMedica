
using ArtMedica.WebApi.Abstractions;
using Microsoft.AspNetCore.Html;
using System.Globalization;

public sealed class DateField : FieldBase
{
    public bool IncludeTime { get; init; }

    public override Task<IHtmlContent> RenderAsync(
        AdminFieldRenderContext context)
    {
        var definition = context.Definition;
        var inputType = IncludeTime ? "datetime-local" : "date";

        var value = context.Value switch
        {
            DateTime dateTime => dateTime.ToString(
                IncludeTime ? "yyyy-MM-ddTHH:mm" : "yyyy-MM-dd",
                CultureInfo.InvariantCulture),

            DateOnly dateOnly => dateOnly.ToString(
                "yyyy-MM-dd",
                CultureInfo.InvariantCulture),

            _ => context.Value?.ToString() ?? ""
        };

        var error = context.Error is null
            ? ""
            : $"<div class=\"text-danger\">{Encode(context.Error)}</div>";

        var html = $"""
        <div class="mb-3">
            <label class="form-label" for="{Encode(definition.Name)}">
                {Encode(definition.Label)}
            </label>

            <input
                type="{inputType}"
                class="form-control"
                id="{Encode(definition.Name)}"
                name="{Encode(definition.Name)}"
                value="{Encode(value)}"
                {(definition.Required ? "required" : "")}
                {(definition.ReadOnly ? "readonly" : "")}
            />

            {error}
        </div>
        """;

        return Task.FromResult<IHtmlContent>(new HtmlString(html));
    }
}
