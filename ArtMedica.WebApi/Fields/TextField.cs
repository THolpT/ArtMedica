using ArtMedica.WebApi.Abstractions;
using Microsoft.AspNetCore.Html;

public sealed class TextField : FieldBase
{
    public string InputType { get; init; } = "text";

    public override Task<IHtmlContent> RenderAsync(
        AdminFieldRenderContext context)
    {
        var definition = context.Definition;

        var html = $"""
        <div class="mb-3">
            <label class="form-label" for="{Encode(definition.Name)}">
                {Encode(definition.Label)}
            </label>

            <input
                type="{Encode(InputType)}"
                class="form-control"
                id="{Encode(definition.Name)}"
                name="{Encode(definition.Name)}"
                value="{Encode(context.Value)}"
                placeholder="{Encode(definition.Placeholder)}"
                {(definition.Required ? "required" : "")}
                {(definition.ReadOnly ? "readonly" : "")}
            />

            {(context.Error is null
                ? ""
                : $"<div class=\"text-danger\">{Encode(context.Error)}</div>")}
        </div>
        """;

        return Task.FromResult<IHtmlContent>(
            new HtmlString(html));
    }
}