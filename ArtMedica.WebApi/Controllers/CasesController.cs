using ArtMedica.WebApi.DTOs;
using Microsoft.AspNetCore.Mvc;
using ArtMedica.Application;
using Microsoft.EntityFrameworkCore;

namespace ArtMedica.WebApi.Controllers
{
    [ApiController]
    [Route("api/cases")]
    public sealed class CasesController(ApplicationDbContext db) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<CaseDto>>> GetAll(CancellationToken ct)
        {
            var cases = await db.Cases
                .AsNoTracking()
                .OrderByDescending(c => c.Date)
                .Select(c => new CaseDto(
                    c.Id,
                    c.ClientName,
                    c.Problem,
                    c.Solution,
                    c.Date,
                    c.Review))
                .ToListAsync(ct);

            return Ok(cases);
        }
    }
}
