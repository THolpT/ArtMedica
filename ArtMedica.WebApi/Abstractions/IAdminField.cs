using ArtMedica.WebApi.Configuration;
using Microsoft.AspNetCore.Html;

namespace ArtMedica.WebApi.Abstractions;

public interface IAdminField
{
    Task<IHtmlContent> RenderAsync(AdminFieldRenderContext context);
}