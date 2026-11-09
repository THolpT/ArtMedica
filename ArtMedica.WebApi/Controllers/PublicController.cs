using ArtMedica.Application;
using ArtMedica.Application.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ArtMedica.Api.Controllers;

[ApiController]
[Route("api/public")]
public class PublicController : ControllerBase
{
    private readonly ApplicationDbContext _db;

    public PublicController(ApplicationDbContext db)
    {
        _db = db;
    }

    // GET api/public/service-categories
    [HttpGet("service-categories")]
    public async Task<ActionResult<IEnumerable<ServiceCategoryDto>>> GetServiceCategories()
    {
        var categories = await _db.ServiceCategories
            .AsNoTracking()
            .OrderBy(c => c.Title)
            .Select(c => new ServiceCategoryDto
            {
                Id = c.Id,
                Title = c.Title,
                ImgUrl = new MediaFileDto
                {
                    Id = c.ImgUrl.Id,
                    Name = c.ImgUrl.Name,
                    Url = c.ImgUrl.Url
                }
            })
            .ToListAsync();

        return Ok(categories);
    }

    // GET api/public/services/by-categories
    // GET api/public/services/by-categories?categoryId=...
    [HttpGet("services/by-categories")]
    public async Task<ActionResult<IEnumerable<CategoryWithServicesDto>>> GetServicesByCategories(
        [FromQuery] Guid? categoryId)
    {
        var query = _db.ServiceCategories
            .AsNoTracking()
            .OrderBy(c => c.Title)
            .AsQueryable();

        if (categoryId.HasValue)
            query = query.Where(c => c.Id == categoryId.Value);

        var categories = await query
            .Select(c => new CategoryWithServicesDto
            {
                Id = c.Id,
                Title = c.Title,
                ImgUrl = new MediaFileDto
                {
                    Id = c.ImgUrl.Id,
                    Name = c.ImgUrl.Name,
                    Url = c.ImgUrl.Url
                },
                Services = c.Services
                    .OrderBy(s => s.DisplayTitle)
                    .Select(s => new ServiceDto
                    {
                        Id = s.Id,
                        NomenclatureCode = s.NomenclatureCode,
                        MISCode = s.MISCode,
                        Duration = s.Duration,
                        Price = s.Price,
                        DisplayTitle = s.DisplayTitle,
                        TechnicalTitle = s.TechnicalTitle,
                        Description = s.Description,
                        Preparation = s.Preparation,
                        Types = s.Types,
                        CategoryId = s.CategoryId
                    }).ToList()
            })
            .ToListAsync();

        return Ok(categories);
    }

    // GET api/public/services/{serviceId}/operation-results
    [HttpGet("services/{serviceId:guid}/operation-results")]
    public async Task<ActionResult<IEnumerable<OperationResultDto>>> GetOperationResultsByService(Guid serviceId)
    {
        var serviceExists = await _db.Services.AnyAsync(s => s.Id == serviceId);
        if (!serviceExists)
            return NotFound(new { message = "Услуга не найдена" });

        var results = await _db.OperationResults
            .AsNoTracking()
            .Where(or => or.OperationId == serviceId)
            .Select(or => new OperationResultDto
            {
                Id = or.Id,
                ImgUrl = new MediaFileDto
                {
                    Id = or.ImgUrl.Id,
                    Name = or.ImgUrl.Name,
                    Url = or.ImgUrl.Url
                },
                OperationId = or.OperationId,
                OperationTitle = or.Operation.TechnicalTitle,
                SpecialistId = or.SpecialistId,
                SpecialistName = or.Specialist.LastName + " " + or.Specialist.FirstName + " " + or.Specialist.MidName
            })
            .ToListAsync();

        return Ok(results);
    }

    // GET api/public/projects
    [HttpGet("projects")]
    public async Task<ActionResult<IEnumerable<ProjectDto>>> GetProjects()
    {
        var projects = await _db.Projects
            .AsNoTracking()
            .OrderByDescending(p => p.Date)
            .Select(p => new ProjectDto
            {
                Id = p.Id,
                Title = p.Title,
                MainImgUrl = new MediaFileDto
                {
                    Id = p.MainImgUrl.Id,
                    Name = p.MainImgUrl.Name,
                    Url = p.MainImgUrl.Url
                },
                FirstDescription = p.FirstDescription,
                SecondDescription = p.SecondDescription,
                SeasonCount = p.SeasonCount,
                MemberCount = p.MemberCount,
                ExpertCount = p.ExpertCount,
                PartnerCount = p.PartnerCount,
                Date = p.Date
            })
            .ToListAsync();

        return Ok(projects);
    }

    // GET api/public/projects/{id}
    [HttpGet("projects/{id:guid}")]
    public async Task<ActionResult<ProjectDetailDto>> GetProject(Guid id)
    {
        var project = await _db.Projects
            .AsNoTracking()
            .Where(p => p.Id == id)
            .Select(p => new ProjectDetailDto
            {
                Id = p.Id,
                Title = p.Title,
                MainImgUrl = new MediaFileDto
                {
                    Id = p.MainImgUrl.Id,
                    Name = p.MainImgUrl.Name,
                    Url = p.MainImgUrl.Url
                },
                FirstDescription = p.FirstDescription,
                SecondDescription = p.SecondDescription,
                SeasonCount = p.SeasonCount,
                MemberCount = p.MemberCount,
                ExpertCount = p.ExpertCount,
                PartnerCount = p.PartnerCount,
                Date = p.Date,
                SlideInfo = p.SlideInfo == null ? null : new ProjectSlideInfoDto
                {
                    Id = p.SlideInfo.Id,
                    Description = p.SlideInfo.Description,
                    ImgUrl = new MediaFileDto
                    {
                        Id = p.SlideInfo.ImgUrl.Id,
                        Name = p.SlideInfo.ImgUrl.Name,
                        Url = p.SlideInfo.ImgUrl.Url
                    },
                    Date = p.SlideInfo.Date,
                    ProjectId = p.SlideInfo.ProjectId
                },
                Stages = p.Stages.ToList(),
                Images = p.Images.Select(i => new MediaFileDto
                {
                    Id = i.Id,
                    Name = i.Name,
                    Url = i.Url
                }).ToList(),
                Formats = p.Formats.Select(f => new ProjectFormatDto
                {
                    Id = f.Id,
                    Title = f.Title,
                    Description = f.Description,
                    Img = new MediaFileDto
                    {
                        Id = f.Img.Id,
                        Name = f.Img.Name,
                        Url = f.Img.Url
                    },
                    ProjectId = f.ProjectId
                }).ToList(),
                Partners = p.Partners.Select(pt => new ProjectPartnerDto
                {
                    Id = pt.Id,
                    Title = pt.Title,
                    Description = pt.Description,
                    ProjectId = pt.ProjectId
                }).ToList()
            })
            .FirstOrDefaultAsync();

        if (project == null)
            return NotFound(new { message = "Проект не найден" });

        return Ok(project);
    }

    // GET api/public/cases
    [HttpGet("cases")]
    public async Task<ActionResult<IEnumerable<CaseDto>>> GetCases()
    {
        var cases = await _db.Cases
            .AsNoTracking()
            .OrderByDescending(c => c.Date)
            .Select(c => new CaseDto
            {
                Id = c.Id,
                ClientName = c.ClientName,
                Problem = c.Problem,
                Solution = c.Solution,
                Date = c.Date,
                Review = c.Review
            })
            .ToListAsync();

        return Ok(cases);
    }

    // GET api/public/promotions
    [HttpGet("promotions")]
    public async Task<ActionResult<IEnumerable<PromotionDto>>> GetPromotions()
    {
        var promotions = await _db.Promotions
            .AsNoTracking()
            .OrderByDescending(p => p.Date)
            .Select(p => new PromotionDto
            {
                Id = p.Id,
                Title = p.Title,
                Description = p.Description,
                Date = p.Date,
                ImgUrl = p.ImgUrl == null ? null : new MediaFileDto
                {
                    Id = p.ImgUrl.Id,
                    Name = p.ImgUrl.Name,
                    Url = p.ImgUrl.Url
                }
            })
            .ToListAsync();

        return Ok(promotions);
    }

    // GET api/public/promotions/{id}
    [HttpGet("promotions/{id:guid}")]
    public async Task<ActionResult<PromotionDto>> GetPromotion(Guid id)
    {
        var promo = await _db.Promotions
            .AsNoTracking()
            .Where(p => p.Id == id)
            .Select(p => new PromotionDto
            {
                Id = p.Id,
                Title = p.Title,
                Description = p.Description,
                Date = p.Date,
                ImgUrl = p.ImgUrl == null ? null : new MediaFileDto
                {
                    Id = p.ImgUrl.Id,
                    Name = p.ImgUrl.Name,
                    Url = p.ImgUrl.Url
                }
            })
            .FirstOrDefaultAsync();

        if (promo == null)
            return NotFound(new { message = "Акция не найдена" });

        return Ok(promo);
    }

    // GET api/public/news
    [HttpGet("news")]
    public async Task<ActionResult<IEnumerable<NewsListDto>>> GetNews()
    {
        var news = await _db.News
            .AsNoTracking()
            .Select(n => new NewsListDto
            {
                Id = n.Id,
                Title = n.Title,
                ImgUrl = new MediaFileDto
                {
                    Id = n.ImgUrl.Id,
                    Name = n.ImgUrl.Name,
                    Url = n.ImgUrl.Url
                },
                Description = n.Description
            })
            .ToListAsync();

        return Ok(news);
    }

    // GET api/public/news/{id}
    [HttpGet("news/{id:guid}")]
    public async Task<ActionResult<NewsDetailDto>> GetNews(Guid id)
    {
        var news = await _db.News
            .AsNoTracking()
            .Where(n => n.Id == id)
            .Select(n => new NewsDetailDto
            {
                Id = n.Id,
                Title = n.Title,
                ImgUrl = new MediaFileDto
                {
                    Id = n.ImgUrl.Id,
                    Name = n.ImgUrl.Name,
                    Url = n.ImgUrl.Url
                },
                Description = n.Description
            })
            .FirstOrDefaultAsync();

        if (news == null)
            return NotFound(new { message = "Новость не найдена" });

        return Ok(news);
    }

    // GET api/public/specialists?department=Хирургия
    [HttpGet("specialists")]
    public async Task<ActionResult<IEnumerable<SpecialistListDto>>> GetSpecialists([FromQuery] string? department)
    {
        var query = _db.Specialists.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(department))
            query = query.Where(s => s.Department == department);

        var specialists = await query
            .Select(s => new SpecialistListDto
            {
                Id = s.Id,
                FullName = s.LastName + " " + s.FirstName + " " + s.MidName,
                Department = s.Department,
                ImgUrl = new MediaFileDto
                {
                    Id = s.Img.Id,
                    Name = s.Img.Name,
                    Url = s.Img.Url
                },
                OperationsCount = s.OperationsCount,
                PositiveReviewsPercentage = s.PositiveReviewsPercentage,
                ActivityArea = s.ActivityArea.ToList()
            })
            .ToListAsync();

        return Ok(specialists);
    }

    // GET api/public/specialists/departments
    [HttpGet("specialists/departments")]
    public async Task<ActionResult<IEnumerable<string>>> GetSpecialistDepartments()
    {
        var departments = await _db.Specialists
            .AsNoTracking()
            .Where(s => s.Department != null)
            .Select(s => s.Department!)
            .Distinct()
            .ToListAsync();

        return Ok(departments);
    }

    // GET api/public/specialists/{id}
    [HttpGet("specialists/{id:guid}")]
    public async Task<ActionResult<SpecialistDetailDto>> GetSpecialist(Guid id)
    {
        var specialist = await _db.Specialists
            .AsNoTracking()
            .Where(s => s.Id == id)
            .Select(s => new SpecialistDetailDto
            {
                Id = s.Id,
                FirstName = s.FirstName,
                MidName = s.MidName,
                LastName = s.LastName,
                FullName = s.LastName + " " + s.FirstName + " " + s.MidName,
                Department = s.Department,
                Description = s.Description,
                ImgUrl = new MediaFileDto
                {
                    Id = s.Img.Id,
                    Name = s.Img.Name,
                    Url = s.Img.Url
                },
                VideoUrl = s.Video == null ? null : new MediaFileDto
                {
                    Id = s.Video.Id,
                    Name = s.Video.Name,
                    Url = s.Video.Url
                },
                StartDate = s.StartDate,
                OperationsCount = s.OperationsCount,
                PositiveReviewsPercentage = s.PositiveReviewsPercentage,
                ActivityArea = s.ActivityArea.ToList(),
                OperationResults = s.operationResults.Select(or => new OperationResultDto
                {
                    Id = or.Id,
                    ImgUrl = new MediaFileDto
                    {
                        Id = or.ImgUrl.Id,
                        Name = or.ImgUrl.Name,
                        Url = or.ImgUrl.Url
                    },
                    OperationId = or.OperationId,
                    OperationTitle = or.Operation.TechnicalTitle,
                    SpecialistId = or.SpecialistId,
                    SpecialistName = s.LastName + " " + s.FirstName + " " + s.MidName
                }).ToList(),
                Qualifications = s.Qualifications.Select(q => new QualificationDto
                {
                    Id = q.Id,
                    Title = q.Title,
                    Level = q.Level,
                    Organization = q.Organization,
                    IssuanceDate = q.IssuanceDate,
                    SpecialistId = q.SpecialistId
                }).ToList(),
                Accreditations = s.Accreditations.Select(a => new AccreditationDto
                {
                    Id = a.Id,
                    DocumentType = a.DocumentType,
                    Specialisation = a.Specialisation,
                    JobTitle = a.JobTitle,
                    IssuanceDate = a.IssuanceDate,
                    ValidityPeriod = a.ValidityPeriod,
                    Description = a.Description,
                    SpecialistId = a.SpecialistId
                }).ToList()
            })
            .FirstOrDefaultAsync();

        if (specialist == null)
            return NotFound(new { message = "Специалист не найден" });

        return Ok(specialist);
    }

    // GET api/public/faqs
    [HttpGet("faqs")]
    public async Task<ActionResult<IEnumerable<FaqDto>>> GetFaqs()
    {
        var faqs = await _db.Faqs
            .AsNoTracking()
            .Select(f => new FaqDto
            {
                Id = f.Id,
                Question = f.Question,
                Answer = f.Answer
            })
            .ToListAsync();

        return Ok(faqs);
    }

    // GET api/public/documents?typeId=...
    [HttpGet("documents")]
    public async Task<ActionResult<IEnumerable<DocumentDto>>> GetDocuments([FromQuery] Guid? typeId)
    {
        var query = _db.Documents
            .AsNoTracking()
            .OrderBy(d => d.Name)
            .AsQueryable();

        if (typeId.HasValue)
            query = query.Where(d => d.Type.Id == typeId.Value);

        var documents = await query
            .Select(d => new DocumentDto
            {
                Id = d.Id,
                Name = d.Name,
                Url = d.Url,
                Type = new DocumentTypeDto
                {
                    Id = d.Type.Id,
                    Name = d.Type.Name
                }
            })
            .ToListAsync();

        return Ok(documents);
    }

    // GET api/public/document-types
    [HttpGet("document-types")]
    public async Task<ActionResult<IEnumerable<DocumentTypeDto>>> GetDocumentTypes()
    {
        var types = await _db.DocumentTypes
            .AsNoTracking()
            .OrderBy(t => t.Name)
            .Select(t => new DocumentTypeDto
            {
                Id = t.Id,
                Name = t.Name
            })
            .ToListAsync();

        return Ok(types);
    }
}