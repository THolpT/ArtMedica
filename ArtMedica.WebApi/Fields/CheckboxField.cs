using ArtMedica.WebApi.Abstractions;
using Microsoft.AspNetCore.Html;

public sealed class CheckboxField : FieldBase
{
    public override Task<IHtmlContent> RenderAsync(
        AdminFieldRenderContext context)
    {
        var definition = context.Definition;

        var checkedValue =
            context.Value is bool b && b
                ? "checked"
                : "";

        var html = $"""
        <div class="form-check mb-3">

            <input
                type="checkbox"
                class="form-check-input"
                id="{Encode(definition.Name)}"
                name="{Encode(definition.Name)}"
                value="true"
                {checkedValue}
            />

            <label class="form-check-label"
                   for="{Encode(definition.Name)}">
                {Encode(definition.Label)}
            </label>

        </div>
        """;

        return Task.FromResult<IHtmlContent>(
            new HtmlString(html));
    }
}