using ArtMedica.WebApi.Abstractions;
using ArtMedica.WebApi.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ArtMedica.WebApi.Pages.Admin;

public class EntityEditModel : PageModel
{
    private readonly IAdminRegistry _registry;
    private readonly IAdminCrudService _crud;

    public EntityEditModel(
        IAdminRegistry registry,
        IAdminCrudService crud)
    {
        _registry = registry;
        _crud = crud;
    }

    public IEntityDefinition Definition { get; private set; } = default!;

    public object EntityItem { get; private set; } = default!;

    [BindProperty(SupportsGet = true)]
    public string Entity { get; set; } = string.Empty;

    [BindProperty(SupportsGet = true)]
    public string Id { get; set; } = string.Empty;

    public string EntityRoute => Entity;

    public async Task<IActionResult> OnGetAsync(
        CancellationToken cancellationToken)
    {
        if (!await LoadEntityAsync(cancellationToken))
            return NotFound();

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(
        CancellationToken cancellationToken)
    {
        if (!await LoadEntityAsync(cancellationToken))
            return NotFound();

        var form = await Request.ReadFormAsync(cancellationToken);

        var values = new Dictionary<string, object?>();

        foreach (var field in Definition.Fields)
        {
            if (field.ReadOnly)
                continue;

            var value = form[field.Name];

            values[field.Name] = value.Count > 0
                ? value.ToString()
                : null;
        }

        try
        {
            await _crud.UpdateAsync(
                Definition,
                EntityItem,
                values,
                HttpContext,
                cancellationToken);

            return RedirectToPage(
                "/Admin/Entity",
                new
                {
                    entity = EntityRoute
                });
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(
                string.Empty,
                ex.Message);

            return Page();
        }
    }

    private async Task<bool> LoadEntityAsync(
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(Entity) ||
            string.IsNullOrWhiteSpace(Id))
        {
            return false;
        }

        try
        {
            Definition = _registry.Get(Entity);
        }
        catch (KeyNotFoundException)
        {
            return false;
        }

        EntityItem = await _crud.GetAsync(
            Definition,
            Id,
            cancellationToken);

        return EntityItem is not null;
    }

    public string? GetError(FieldDefinition field)
    {
        return ModelState[field.Name]?
            .Errors
            .FirstOrDefault()?
            .ErrorMessage;
    }

    public object? GetValue(FieldDefinition field)
    {
        return field.GetValue(EntityItem);
    }
}