
using ArtMedica.WebApi.Abstractions;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ArtMedica.WebApi.Pages.Admin;

public class IndexModel : PageModel
{
    private readonly IAdminRegistry _registry;

    public IReadOnlyCollection<IEntityDefinition> Entities { get; private set; }
        = [];

    public IndexModel(IAdminRegistry registry)
    {
        _registry = registry;
    }

    public void OnGet()
    {
        Entities = _registry.Entities;
    }
}