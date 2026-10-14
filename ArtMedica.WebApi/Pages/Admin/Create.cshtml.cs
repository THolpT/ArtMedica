using ArtMedica.WebApi.Abstractions;
using ArtMedica.WebApi.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ArtMedica.WebApi.Pages.Admin;
public class CreateModel : PageModel
{
    private readonly IAdminRegistry _registry;
    private readonly IAdminCrudService _crud;

    public CreateModel(
        IAdminRegistry registry,
        IAdminCrudService crud)
    {
        _registry = registry;
        _crud = crud;
    }

    public IEntityDefinition Definition { get; private set; } = default!;

    [BindProperty(SupportsGet = true)]
    public string Entity { get; set; } = string.Empty;

    public IReadOnlyCollection<IEntityDefinition> Entities { get; private set; } = [];

    public string EntityRoute => Entity;

    public async Task<IActionResult> OnGetAsync(
        CancellationToken cancellationToken)
    {
        if (!TryLoadDefinition())
            return NotFound();

        Entities = _registry.Entities;

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(
        CancellationToken cancellationToken)
    {
        if (!TryLoadDefinition())
            return NotFound();

        var form = await Request.ReadFormAsync(cancellationToken);

        var values = new Dictionary<string, object?>();

        foreach (var field in Definition.Fields)
        {
            if (field.ReadOnly)
                continue;

            var value = form[field.Name];

            values[field.Name] =
                value.Count > 0
                    ? value.ToString()
                    : null;
        }

        try
        {
            await _crud.CreateAsync(
                Definition,
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

    private bool TryLoadDefinition()
    {
        if (string.IsNullOrWhiteSpace(Entity))
            return false;

        try
        {
            Definition = _registry.Get(Entity);
            return true;
        }
        catch (KeyNotFoundException)
        {
            return false;
        }
    }

    public string? GetError(FieldDefinition field)
    {
        var error = ModelState[field.Name]
            ?.Errors
            .FirstOrDefault();

        return error?.ErrorMessage;
    }
}