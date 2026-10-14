using ArtMedica.WebApi.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ArtMedica.WebApi.Pages.Admin;
public class ListModel : PageModel
{
    private readonly IAdminRegistry _registry;
    private readonly IAdminCrudService _crud;

    public ListModel(
        IAdminRegistry registry,
        IAdminCrudService crud)
    {
        _registry = registry;
        _crud = crud;
    }

    public IEntityDefinition Definition { get; private set; } = default!;

    public IReadOnlyList<object> Items { get; private set; } = [];

    [BindProperty(SupportsGet = true)]
    public string Entity { get; set; } = string.Empty;

    public string EntityRoute => Entity;

    public IReadOnlyCollection<IEntityDefinition> Entities { get; private set; } = [];

    public async Task<IActionResult> OnGetAsync(
        CancellationToken cancellationToken)
    {
        try
        {
            Definition = _registry.Get(Entity);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }

        Items = await _crud.GetListAsync(
            Definition,
            cancellationToken);
        
        Entities = _registry.Entities;

        return Page();
    }

    public object? GetPropertyValue(
        object entity,
        string propertyName)
    {
        return entity
            .GetType()
            .GetProperty(propertyName)?
            .GetValue(entity);
    }

    public object? GetId(object entity)
    {
        var property = Definition.EntityType
            .GetProperties()
            .FirstOrDefault(x =>
                string.Equals(
                    x.Name,
                    "Id",
                    StringComparison.OrdinalIgnoreCase));

        return property?.GetValue(entity);
    }

    public async Task<IActionResult> OnPostDeleteAsync(
    string id,
    CancellationToken cancellationToken)
{
    try
    {
        Definition = _registry.Get(Entity);

        await _crud.DeleteAsync(
            Definition,
            id,
            cancellationToken);

        return RedirectToPage(
            "/Admin/Entity",
            new { entity = EntityRoute });
    }
    catch (KeyNotFoundException)
    {
        return NotFound();
    }
}
}