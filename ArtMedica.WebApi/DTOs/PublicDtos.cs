using System;
using System.Collections.Generic;

namespace ArtMedica.Application.DTOs;

public class MediaFileDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Url { get; set; } = null!;
}

public class ServiceCategoryDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public MediaFileDto ImgUrl { get; set; } = null!;
}

public class CategoryWithServicesDto : ServiceCategoryDto
{
    public List<ServiceDto> Services { get; set; } = new();
}

public class ServiceDto
{
    public Guid Id { get; set; }
    public string NomenclatureCode { get; set; } = null!;
    public string MISCode { get; set; } = null!;
    public int Duration { get; set; }
    public decimal Price { get; set; }
    public string DisplayTitle { get; set; } = null!;
    public string TechnicalTitle { get; set; } = null!;
    public string? Description { get; set; }
    public string? Preparation { get; set; }
    public string? Types { get; set; }
    public Guid CategoryId { get; set; }
}

public class OperationResultDto
{
    public Guid Id { get; set; }
    public MediaFileDto ImgUrl { get; set; } = null!;
    public Guid OperationId { get; set; }
    public string OperationTitle { get; set; } = null!;
    public Guid SpecialistId { get; set; }
    public string SpecialistName { get; set; } = null!;
}

// ---------- ПРОЕКТЫ ----------

public class ProjectFormatDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public MediaFileDto Img { get; set; } = null!;
    public Guid ProjectId { get; set; }
}

public class ProjectPartnerDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public Guid ProjectId { get; set; }
}

public class ProjectDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public MediaFileDto MainImgUrl { get; set; } = null!;
    public string? FirstDescription { get; set; }
    public string? SecondDescription { get; set; }
    public int SeasonCount { get; set; }
    public int MemberCount { get; set; }
    public int ExpertCount { get; set; }
    public int PartnerCount { get; set; }
    public DateTime Date { get; set; }
}

public class ProjectSlideInfoDto
{
    public Guid Id { get; set; }
    public string? Description { get; set; }
    public MediaFileDto ImgUrl { get; set; } = null!;
    public DateTime Date { get; set; }
    public Guid ProjectId { get; set; }
}

public class ProjectDetailDto : ProjectDto
{
    public ProjectSlideInfoDto? SlideInfo { get; set; }
    public List<string> Stages { get; set; } = new();
    public List<MediaFileDto> Images { get; set; } = new();
    public List<ProjectFormatDto> Formats { get; set; } = new();
    public List<ProjectPartnerDto> Partners { get; set; } = new();
}

// ---------- СПЕЦИАЛИСТЫ ----------

public class QualificationDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public string Level { get; set; } = null!;
    public string Organization { get; set; } = null!;
    public DateTime IssuanceDate { get; set; }
    public Guid SpecialistId { get; set; }
}

public class AccreditationDto
{
    public Guid Id { get; set; }
    public string DocumentType { get; set; } = null!;
    public string Specialisation { get; set; } = null!;
    public string JobTitle { get; set; } = null!;
    public DateTime IssuanceDate { get; set; }
    public DateTime ValidityPeriod { get; set; }
    public string? Description { get; set; }
    public Guid SpecialistId { get; set; }
}

public class SpecialistListDto
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = null!;
    public string? Department { get; set; }
    public MediaFileDto ImgUrl { get; set; } = null!;
    public int OperationsCount { get; set; }
    public byte PositiveReviewsPercentage { get; set; }
    public List<string> ActivityArea { get; set; } = new();
}

public class SpecialistDetailDto : SpecialistListDto
{
    public string FirstName { get; set; } = null!;
    public string MidName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string? Description { get; set; }
    public MediaFileDto? VideoUrl { get; set; }
    public DateTime StartDate { get; set; }
    public List<OperationResultDto> OperationResults { get; set; } = new();
    public List<QualificationDto> Qualifications { get; set; } = new();
    public List<AccreditationDto> Accreditations { get; set; } = new();
}

// ---------- КЕЙСЫ / АКЦИИ / НОВОСТИ ----------

public class CaseDto
{
    public Guid Id { get; set; }
    public string ClientName { get; set; } = null!;
    public string Problem { get; set; } = null!;
    public string Solution { get; set; } = null!;
    public DateTime Date { get; set; }
    public int Review { get; set; }
}

public class PromotionDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public DateTime Date { get; set; }
    public MediaFileDto? ImgUrl { get; set; }
}

public class NewsListDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public MediaFileDto ImgUrl { get; set; } = null!;
    public string? Description { get; set; }
}

public class NewsDetailDto : NewsListDto
{
}

// ---------- FAQ ----------

public class FaqDto
{
    public Guid Id { get; set; }
    public string Question { get; set; } = null!;
    public string Answer { get; set; } = null!;
}

// ---------- ДОКУМЕНТЫ ----------

public class DocumentTypeDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
}

public class DocumentDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public DocumentTypeDto Type { get; set; } = null!;
    public string Url { get; set; } = null!;
}