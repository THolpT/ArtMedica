using ArtMedica.Domain.Models;
using ArtMedica.WebApi.Abstractions;
using Microsoft.Extensions.DependencyInjection;

// Добавь using с фактическим пространством имён ApplicationDbContext.
using ArtMedica.WebApi.Services;
using ArtMedica.Application;

namespace ArtMedica.WebApi.Configuration;

public static class AdminRegistration
{
    public static IServiceCollection AddArtMedicaAdmin(
        this IServiceCollection services)
    {
        services.AddAdmin<ApplicationDbContext>(admin =>
        {
            // =============================================================
            // Медиафайлы
            // =============================================================
            admin.Entity<MediaFile>(entity =>
            {
                entity.Name = "media-files";
                entity.Label = "Медиафайлы";
                entity.Route = "media-files";

                entity.Field("id", x => x.Id)
                    .Label("ID")
                    .Field(new TextField())
                    .ReadOnly();

                entity.Field("name", x => x.Name)
                    .Label("Название")
                    .Field(new TextField())
                    .Required()
                    .Placeholder("Введите название файла");

                entity.Field("url", x => x.Url)
                    .Label("Файл")
                    .Field(new FileField())
                    .Required();

                // УБРАНО поле "type" — MediaFile.Type больше нет в модели.

                entity.Create(async context =>
                {
                    var file = context.HttpContext.Request.Form.Files.GetFile("Url");

                    if (file is null || file.Length == 0)
                        throw new InvalidOperationException("Выберите файл для загрузки.");

                    var storage = context.GetService<MediaStorageService>();
                    var uploaded = await storage.SaveAsync(file, context.CancellationToken);

                    var entity = context.Entity;
                    entity.Name = Path.GetFileNameWithoutExtension(file.FileName);
                    entity.Url = uploaded.Url;
                    // УБРАНО: entity.Type = uploaded.Type;

                    return entity;
                });
            });

            // =============================================================
            // Документы
            // =============================================================
            admin.Entity<DocumentType>(entity =>
            {
                entity.Name = "document-types";
                entity.Label = "Типы документов";
                entity.Route = "document-types";

                entity.Field("id", x => x.Id)
                    .Label("ID").Field(new TextField()).ReadOnly();

                entity.Field("name", x => x.Name)
                    .Label("Название").Field(new TextField()).Required();

                entity.Create(async context => context.Entity);
            });

            admin.Entity<Document>(entity =>
{
    entity.Name = "documents";
    entity.Label = "Документы";
    entity.Route = "documents";

    entity.Field("id", x => x.Id)
        .Label("ID").Field(new TextField()).ReadOnly();

    entity.Field("name", x => x.Name)
        .Label("Название").Field(new TextField()).Required();

    entity.Field("typeId", x => x.TypeId)
        .Label("Тип документа")
        .Field(new ReferenceField<DocumentType>("document-types"))
        .Required();

    entity.Field("url", x => x.Url)
        .Label("Файл")
        .Field(new FileField())
        .Required();

    entity.Create(async context =>
    {
        var form = context.HttpContext.Request.Form;
        var file = form.Files.GetFile("url");
        var entity = context.Entity;

        // ReferenceField обычно сам проставляет TypeId,
        // но оставляем страховку на случай нестандартного рендера.
        if (entity.TypeId == Guid.Empty)
        {
            if (Guid.TryParse(form["typeId"], out var typeId))
                entity.TypeId = typeId;
            else
                throw new InvalidOperationException("Не выбран тип документа.");
        }

        // Файл: загружаем, если пришёл, иначе используем уже сохранённый Url
        if (file is not null && file.Length > 0)
        {
            var storage = context.GetService<DocumentStorageService>();
            var uploaded = await storage.SaveAsync(file, context.CancellationToken);
            entity.Url = uploaded.Url;
        }
        else if (string.IsNullOrWhiteSpace(entity.Url))
        {
            throw new InvalidOperationException("Загрузите файл документа.");
        }

        return entity;
    });
});

            // =============================================================
            // FAQ
            // =============================================================
            admin.Entity<Faq>(entity =>
            {
                entity.Name = "faqs";
                entity.Label = "FAQ";
                entity.Route = "faqs";

                entity.Field("id", x => x.Id)
                    .Label("ID").Field(new TextField()).ReadOnly();

                entity.Field("question", x => x.Question)
                    .Label("Вопрос").Field(new TextField()).Required();

                entity.Field("answer", x => x.Answer)
                    .Label("Ответ").Field(new WysiwygField());

                entity.Create(async context => context.Entity);
            });

            // =============================================================
            // Кейсы
            // =============================================================
            admin.Entity<Case>(entity =>
            {
                entity.Name = "cases";
                entity.Label = "Кейсы";
                entity.Route = "cases";

                entity.Field("id", x => x.Id)
                    .Label("ID").Field(new TextField()).ReadOnly();

                entity.Field("clientName", x => x.ClientName)
                    .Label("Имя клиента").Field(new TextField()).Required();

                entity.Field("problem", x => x.Problem)
                    .Label("Проблема").Field(new TextField()).Required();

                entity.Field("solution", x => x.Solution)
                    .Label("Решение").Field(new TextField()).Required();

                entity.Field("date", x => x.Date)
                    .Label("Дата").Field(new DateField()).Required();

                entity.Field("review", x => x.Review)
                    .Label("Оценка")
                    .Field(new IntField { Min = 0, Max = 5 })
                    .Required();

                entity.Create(async context => context.Entity);
            });

            // =============================================================
            // Новости
            // =============================================================
            admin.Entity<New>(entity =>
            {
                entity.Name = "news";
                entity.Label = "Новости";
                entity.Route = "news";

                entity.Field("id", x => x.Id)
                    .Label("ID").Field(new TextField()).ReadOnly();

                entity.Field("title", x => x.Title)
                    .Label("Заголовок").Field(new TextField()).Required();

                entity.Field("imgUrlId", x => x.ImgUrlId)
                    .Label("Изображение")
                    .Field(new ReferenceField<MediaFile>("media-files"))
                    .Required();

                entity.Field("description", x => x.Description)
                    .Label("Описание").Field(new TextField());

                entity.Create(async context => context.Entity);
            });

            // =============================================================
            // Результаты операций
            // =============================================================
            admin.Entity<OperationResult>(entity =>
            {
                entity.Name = "operation-results";
                entity.Label = "Результаты операций";
                entity.Route = "operation-results";

                entity.Field("id", x => x.Id)
                    .Label("ID").Field(new TextField()).ReadOnly();

                entity.Field("imgUrlId", x => x.ImgUrlId)
                    .Label("Изображение")
                    .Field(new ReferenceField<MediaFile>("media-files"))
                    .Required();

                entity.Field("operationId", x => x.OperationId)
                    .Label("Услуга")
                    .Field(new ReferenceField<Service>("services"))
                    .Required();

                entity.Field("specialistId", x => x.SpecialistId)
                    .Label("Специалист")
                    .Field(new ReferenceField<Specialist>("specialists"))
                    .Required();

                entity.Create(async context => context.Entity);
            });

            // =============================================================
            // Проекты
            // =============================================================
            admin.Entity<Project>(entity =>
            {
                entity.Name = "projects";
                entity.Label = "Проекты";
                entity.Route = "projects";

                entity.Field("id", x => x.Id)
                    .Label("ID").Field(new TextField()).ReadOnly();

                entity.Field("title", x => x.Title)
                    .Label("Название").Field(new TextField()).Required();

                entity.Field("mainImgUrlId", x => x.MainImgUrlId)
                    .Label("Основное изображение")
                    .Field(new ReferenceField<MediaFile>("media-files"))
                    .Required();

                entity.Field("firstDescription", x => x.FirstDescription)
                    .Label("Первое описание").Field(new TextField());

                entity.Field("secondDescription", x => x.SecondDescription)
                    .Label("Второе описание").Field(new TextField());

                // НОВОЕ: дата проекта
                entity.Field("date", x => x.Date)
                    .Label("Дата").Field(new DateField()).Required();

                entity.Field("seasonCount", x => x.SeasonCount)
                    .Label("Количество сезонов")
                    .Field(new IntField { Min = 0 })
                    .Required();

                entity.Field("memberCount", x => x.MemberCount)
                    .Label("Количество участников")
                    .Field(new IntField { Min = 0 })
                    .Required();

                entity.Field("expertCount", x => x.ExpertCount)
                    .Label("Количество экспертов")
                    .Field(new IntField { Min = 0 })
                    .Required();

                entity.Field("partnerCount", x => x.PartnerCount)
                    .Label("Количество партнёров")
                    .Field(new IntField { Min = 0 })
                    .Required();

                entity.Field("projectStages", x => x.Stages)
                    .Label("Стадии")
                    .Field(new StringCollectionField());

                entity.Create(async context => context.Entity);
            });

            // =============================================================
            // Слайды проектов
            // =============================================================
            admin.Entity<ProjectSlideInfo>(entity =>
            {
                entity.Name = "project-slide-infos";
                entity.Label = "Слайды проектов";
                entity.Route = "project-slide-infos";

                entity.Field("id", x => x.Id)
                    .Label("ID").Field(new TextField()).ReadOnly();

                entity.Field("description", x => x.Description)
                    .Label("Описание").Field(new TextField());

                entity.Field("imgUrlId", x => x.ImgUrlId)
                    .Label("Изображение")
                    .Field(new ReferenceField<MediaFile>("media-files"))
                    .Required();

                entity.Field("projectId", x => x.ProjectId)
                    .Label("Проект")
                    .Field(new ReferenceField<Project>("projects"))
                    .Required();

                entity.Field("date", x => x.Date)
                    .Label("Дата").Field(new DateField()).Required();

                entity.Create(async context => context.Entity);
            });

            // =============================================================
            // Акции
            // =============================================================
            admin.Entity<Promotion>(entity =>
            {
                entity.Name = "promotions";
                entity.Label = "Акции";
                entity.Route = "promotions";

                entity.Field("id", x => x.Id)
                    .Label("ID").Field(new TextField()).ReadOnly();

                entity.Field("title", x => x.Title)
                    .Label("Название").Field(new TextField()).Required();

                // НОВОЕ: изображение акции
                entity.Field("imgUrlId", x => x.ImgUrlId)
                    .Label("Изображение")
                    .Field(new ReferenceField<MediaFile>("media-files"))
                    .Required();

                entity.Field("description", x => x.Description)
                    .Label("Описание").Field(new WysiwygField());

                entity.Field("date", x => x.Date)
                    .Label("Дата").Field(new DateField()).Required();

                entity.Create(async context => context.Entity);
            });

            // =============================================================
            // Услуги
            // =============================================================
            admin.Entity<Service>(entity =>
            {
                entity.Name = "services";
                entity.Label = "Услуги";
                entity.Route = "services";

                entity.Field("id", x => x.Id)
                    .Label("ID").Field(new TextField()).ReadOnly();

                entity.Field("nomenclatureCode", x => x.NomenclatureCode)
                    .Label("Код номенклатуры")
                    .Field(new TextField()).Required();

                entity.Field("misCode", x => x.MISCode)
                    .Label("Код МИС")
                    .Field(new TextField()).Required();

                entity.Field("duration", x => x.Duration)
                    .Label("Длительность, мин.")
                    .Field(new IntField { Min = 0 }).Required();

                entity.Field("price", x => x.Price)
                    .Label("Цена")
                    .Field(new TextField()).Required();

                entity.Field("displayTitle", x => x.DisplayTitle)
                    .Label("Название для отображения")
                    .Field(new TextField()).Required();

                entity.Field("technicalTitle", x => x.TechnicalTitle)
                    .Label("Техническое название")
                    .Field(new TextField()).Required();

                entity.Field("description", x => x.Description)
                    .Label("Описание").Field(new TextField());

                entity.Field("preparation", x => x.Preparation)
                    .Label("Подготовка").Field(new TextField());

                entity.Field("types", x => x.Types)
                    .Label("Типы").Field(new TextField());

                entity.Field("categoryId", x => x.CategoryId)
                    .Label("Категория")
                    .Field(new ReferenceField<ServiceCategory>("service-categories"))
                    .Required();

                entity.Create(async context => context.Entity);
            });

            // =============================================================
            // Категории услуг
            // =============================================================
            admin.Entity<ServiceCategory>(entity =>
            {
                entity.Name = "service-categories";
                entity.Label = "Категории услуг";
                entity.Route = "service-categories";

                entity.Field("id", x => x.Id)
                    .Label("ID").Field(new TextField()).ReadOnly();

                entity.Field("title", x => x.Title)
                    .Label("Название").Field(new TextField()).Required();

                entity.Field("imgUrlId", x => x.ImgUrlId)
                    .Label("Изображение")
                    .Field(new ReferenceField<MediaFile>("media-files"))
                    .Required();

                entity.Create(async context => context.Entity);
            });

            // =============================================================
            // Форматы проектов
            // =============================================================
            admin.Entity<ProjectFormat>(entity =>
            {
                entity.Name = "project-formats";
                entity.Label = "Форматы проектов";
                entity.Route = "project-formats";

                entity.Field("id", x => x.Id)
                    .Label("ID").Field(new TextField()).ReadOnly();

                entity.Field("title", x => x.Title)
                    .Label("Название").Field(new TextField()).Required();

                entity.Field("description", x => x.Description)
                    .Label("Описание").Field(new TextField());

                entity.Field("imgId", x => x.ImgId)
                    .Label("Изображение")
                    .Field(new ReferenceField<MediaFile>("media-files"))
                    .Required();

                entity.Field("projectId", x => x.ProjectId)
                    .Label("Проект")
                    .Field(new ReferenceField<Project>("projects"))
                    .Required();

                entity.Create(async context =>
                {
                    var form = context.HttpContext.Request.Form;
                    var entity = context.Entity;

                    if (Guid.TryParse(form["projectId"], out var projectId))
                        entity.ProjectId = projectId;
                    else
                        throw new InvalidOperationException("Не выбран проект.");

                    if (Guid.TryParse(form["imgId"], out var imgId))
                        entity.ImgId = imgId;
                    else
                        throw new InvalidOperationException("Не выбрано изображение.");

                    return entity;
                });
            });

            // =============================================================
            // Партнёры проектов
            // =============================================================
            admin.Entity<ProjectPartner>(entity =>
            {
                entity.Name = "project-partners";
                entity.Label = "Партнёры проектов";
                entity.Route = "project-partners";

                entity.Field("id", x => x.Id)
                    .Label("ID").Field(new TextField()).ReadOnly();

                entity.Field("title", x => x.Title)
                    .Label("Название").Field(new TextField()).Required();

                entity.Field("description", x => x.Description)
                    .Label("Описание").Field(new TextField());

                entity.Field("projectId", x => x.ProjectId)
                    .Label("Проект")
                    .Field(new ReferenceField<Project>("projects"))
                    .Required();

                entity.Create(async context => context.Entity);
            });

            // =============================================================
            // Квалификации специалистов
            // =============================================================
            admin.Entity<Qualification>(entity =>
            {
                entity.Name = "qualifications";
                entity.Label = "Квалификации";
                entity.Route = "qualifications";

                entity.Field("id", x => x.Id)
                    .Label("ID").Field(new TextField()).ReadOnly();

                entity.Field("title", x => x.Title)
                    .Label("Название").Field(new TextField()).Required();

                entity.Field("level", x => x.Level)
                    .Label("Уровень").Field(new TextField()).Required();

                entity.Field("organization", x => x.Organization)
                    .Label("Организация").Field(new TextField()).Required();

                entity.Field("issuanceDate", x => x.IssuanceDate)
                    .Label("Дата выдачи").Field(new DateField()).Required();

                entity.Field("specialistId", x => x.SpecialistId)
                    .Label("Специалист")
                    .Field(new ReferenceField<Specialist>("specialists"))
                    .Required();

                entity.Create(async context => context.Entity);
            });

            // =============================================================
            // Аккредитации специалистов
            // =============================================================
            admin.Entity<Accreditation>(entity =>
            {
                entity.Name = "accreditations";
                entity.Label = "Аккредитации";
                entity.Route = "accreditations";

                entity.Field("id", x => x.Id)
                    .Label("ID").Field(new TextField()).ReadOnly();

                entity.Field("documentType", x => x.DocumentType)
                    .Label("Тип документа").Field(new TextField()).Required();

                entity.Field("specialisation", x => x.Specialisation)
                    .Label("Специализация").Field(new TextField()).Required();

                entity.Field("jobTitle", x => x.JobTitle)
                    .Label("Должность").Field(new TextField()).Required();

                entity.Field("issuanceDate", x => x.IssuanceDate)
                    .Label("Дата выдачи").Field(new DateField()).Required();

                entity.Field("validityPeriod", x => x.ValidityPeriod)
                    .Label("Срок действия").Field(new DateField()).Required();

                entity.Field("description", x => x.Description)
                    .Label("Описание").Field(new TextField());

                entity.Field("specialistId", x => x.SpecialistId)
                    .Label("Специалист")
                    .Field(new ReferenceField<Specialist>("specialists"))
                    .Required();

                entity.Create(async context => context.Entity);
            });

            // =============================================================
            // Специалисты
            // =============================================================
            admin.Entity<Specialist>(entity =>
            {
                entity.Name = "specialists";
                entity.Label = "Специалисты";
                entity.Route = "specialists";

                entity.Field("id", x => x.Id)
                    .Label("ID").Field(new TextField()).ReadOnly();

                entity.Field("firstName", x => x.FirstName)
                    .Label("Имя").Field(new TextField()).Required();

                entity.Field("midName", x => x.MidName)
                    .Label("Отчество").Field(new TextField()).Required();

                entity.Field("lastName", x => x.LastName)
                    .Label("Фамилия").Field(new TextField()).Required();

                entity.Field("department", x => x.Department)
                    .Label("Отделение").Field(new TextField());

                entity.Field("description", x => x.Description)
                    .Label("Описание").Field(new TextField());

                entity.Field("imgId", x => x.ImgId)
                    .Label("Фотография")
                    .Field(new ReferenceField<MediaFile>("media-files"))
                    .Required();

                entity.Field("videoId", x => x.VideoId)
                    .Label("Видео")
                    .Field(new ReferenceField<MediaFile>("media-files"))
                    .Required();

                entity.Field("startDate", x => x.StartDate)
                    .Label("Дата начала работы")
                    .Field(new DateField()).Required();

                entity.Field("operationsCount", x => x.OperationsCount)
                    .Label("Количество операций")
                    .Field(new IntField { Min = 0 }).Required();

                entity.Field(
                    "positiveReviewsPercentage",
                    x => x.PositiveReviewsPercentage)
                    .Label("Процент положительных отзывов")
                    .Field(new IntField { Min = 0, Max = 100 })
                    .Required();

                entity.Create(async context => context.Entity);
            });
        });

        return services;
    }
}