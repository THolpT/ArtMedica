using ArtMedica.Application;
using ArtMedica.WebApi.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ArtMedica.WebApi.Controllers
{
    [ApiController]
    [Route("api/service-categories")]
    public sealed class ServiceCategoriesController(ApplicationDbContext db) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<ServiceCategoryDto>>> GetAll(CancellationToken ct)
        {
            var categories = await db.ServiceCategories
                .AsNoTracking()
                .OrderBy(c => c.Title)
                .Select(c => new ServiceCategoryDto(c.Id, c.Title))
                .ToListAsync(ct);

            return Ok(categories);
        }
    }
}
