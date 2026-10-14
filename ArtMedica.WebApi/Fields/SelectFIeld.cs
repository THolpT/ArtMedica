using ArtMedica.WebApi.Abstractions;
using Microsoft.AspNetCore.Html;

public sealed class SelectField : FieldBase
{
    public override Task<IHtmlContent> RenderAsync(
        AdminFieldRenderContext context)
    {
        var definition = context.Definition;
        var value = context.Value?.ToString();

        var options = string.Join(
            Environment.NewLine,
            definition.Options.Select(x =>
                $"""
                <option value="{Encode(x.Value)}"
                    {(x.Value == value ? "selected" : "")}>
                    {Encode(x.Text)}
                </option>
                """));

        var html = $"""
        <div class="mb-3">
            <label class="form-label">
                {Encode(definition.Label)}
            </label>

            <select
                class="form-select"
                name="{Encode(definition.Name)}"
                {(definition.Required ? "required" : "")}
                {(definition.ReadOnly ? "disabled" : "")}
            >
                {options}
            </select>
        </div>
        """;

        return Task.FromResult<IHtmlContent>(
            new HtmlString(html));
    }
}