using ArtMedica.Application;
using ArtMedica.WebApi.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ArtMedica.WebApi.Controllers
{
    [ApiController]
    [Route("api/promotions")]
    public sealed class PromotionsController(ApplicationDbContext db) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<PromotionDto>>> GetAll(CancellationToken ct)
        {
            var promotions = await db.Promotions
                .AsNoTracking()
                .OrderByDescending(p => p.Date)
                .Select(p => new PromotionDto(
                    p.Id,
                    p.Title,
                    p.Description,
                    p.Date))
                .ToListAsync(ct);

            return Ok(promotions);
        }
    }
}
