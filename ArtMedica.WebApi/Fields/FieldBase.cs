using ArtMedica.WebApi.Abstractions;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

public abstract class FieldBase : IAdminField
{
    public abstract Task<IHtmlContent> RenderAsync(
        AdminFieldRenderContext context);

    protected static string Encode(object? value)
    {
        return System.Net.WebUtility.HtmlEncode(value?.ToString() ?? "");
    }
}