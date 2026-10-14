
using ArtMedica.WebApi.Abstractions;
using Microsoft.AspNetCore.Html;

public sealed class FileField : FieldBase
{
    public override Task<IHtmlContent> RenderAsync(
        AdminFieldRenderContext context)
    {
        var name = Encode(context.Definition.Name);
        var label = Encode(context.Definition.Label);

        var html = $"""
            <div class="mb-3">
                <label class="form-label" for="{name}">
                    {label}
                </label>

                <input
                    class="form-control"
                    type="file"
                    id="{name}"
                    name="{name}"
                    accept="image/*,video/*"
                    {(context.Definition.Required ? "required" : "")} />

                {(context.Error is not null
                    ? $"<div class=\"text-danger\">{Encode(context.Error)}</div>"
                    : "")}
            </div>
            """;

        return Task.FromResult<IHtmlContent>(new HtmlString(html));
    }
}
