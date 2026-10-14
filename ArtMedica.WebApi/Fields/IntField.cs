
using ArtMedica.WebApi.Abstractions;
using Microsoft.AspNetCore.Html;

public sealed class IntField : FieldBase
{
    public int Min { get; init; } = int.MinValue;
    public int Max { get; init; } = int.MaxValue;

    public override Task<IHtmlContent> RenderAsync(
        AdminFieldRenderContext context)
    {
        var definition = context.Definition;
        var value = context.Value?.ToString() ?? "";

        var error = context.Error is null
            ? ""
            : $"<div class=\"text-danger\">{Encode(context.Error)}</div>";

        var html = $"""
        <div class="mb-3">
            <label class="form-label" for="{Encode(definition.Name)}">
                {Encode(definition.Label)}
            </label>

            <input
                type="number"
                step="1"
                min="{Min}"
                max="{Max}"
                class="form-control"
                id="{Encode(definition.Name)}"
                name="{Encode(definition.Name)}"
                value="{Encode(value)}"
                placeholder="{Encode(definition.Placeholder)}"
                {(definition.Required ? "required" : "")}
                {(definition.ReadOnly ? "readonly" : "")}
            />

            {error}
        </div>
        """;

        return Task.FromResult<IHtmlContent>(new HtmlString(html));
    }
}
