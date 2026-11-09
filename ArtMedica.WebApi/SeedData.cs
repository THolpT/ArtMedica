using ArtMedica.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace ArtMedica.Application.Seeding;

public static class DbSeeder
{
    // ====================================================================
    //  ДЕТЕРМИНИРОВАННЫЕ ИДЕНТИФИКАТОРЫ (чтобы повторный запуск был безопасен)
    // ====================================================================

    private static class Media
    {
        public static readonly Guid CatCosmetology      = new("a1000000-0000-0000-0000-000000000001");
        public static readonly Guid CatDermatology      = new("a1000000-0000-0000-0000-000000000002");
        public static readonly Guid CatLor              = new("a1000000-0000-0000-0000-000000000003");
        public static readonly Guid CatPlastic          = new("a1000000-0000-0000-0000-000000000004");
        public static readonly Guid CatGynecology       = new("a1000000-0000-0000-0000-000000000005");
        public static readonly Guid CatMassage          = new("a1000000-0000-0000-0000-000000000006");

        public static readonly Guid Spec1Photo          = new("a1100000-0000-0000-0000-000000000001");
        public static readonly Guid Spec1Video          = new("a1100000-0000-0000-0000-000000000002");
        public static readonly Guid Spec2Photo          = new("a1100000-0000-0000-0000-000000000003");
        public static readonly Guid Spec2Video          = new("a1100000-0000-0000-0000-000000000004");
        public static readonly Guid Spec3Photo          = new("a1100000-0000-0000-0000-000000000005");
        public static readonly Guid Spec3Video          = new("a1100000-0000-0000-0000-000000000006");
        public static readonly Guid Spec4Photo          = new("a1100000-0000-0000-0000-000000000007");
        public static readonly Guid Spec4Video          = new("a1100000-0000-0000-0000-000000000008");

        public static readonly Guid Project1Main        = new("a1200000-0000-0000-0000-000000000001");
        public static readonly Guid Project1Slide       = new("a1200000-0000-0000-0000-000000000002");
        public static readonly Guid Project2Main        = new("a1200000-0000-0000-0000-000000000003");
        public static readonly Guid Project2Slide       = new("a1200000-0000-0000-0000-000000000004");
        public static readonly Guid Project3Main        = new("a1200000-0000-0000-0000-000000000005");
        public static readonly Guid Project3Slide       = new("a1200000-0000-0000-0000-000000000006");

        public static readonly Guid Format1Img          = new("a1300000-0000-0000-0000-000000000001");
        public static readonly Guid Format2Img          = new("a1300000-0000-0000-0000-000000000002");
        public static readonly Guid Format3Img          = new("a1300000-0000-0000-0000-000000000003");

        public static readonly Guid News1               = new("a1400000-0000-0000-0000-000000000001");
        public static readonly Guid News2               = new("a1400000-0000-0000-0000-000000000002");
        public static readonly Guid News3               = new("a1400000-0000-0000-0000-000000000003");
        public static readonly Guid News4               = new("a1400000-0000-0000-0000-000000000004");

        public static readonly Guid Promo1              = new("a1500000-0000-0000-0000-000000000001");
        public static readonly Guid Promo2              = new("a1500000-0000-0000-0000-000000000002");
        public static readonly Guid Promo3              = new("a1500000-0000-0000-0000-000000000003");

        public static readonly Guid Op1                 = new("a1600000-0000-0000-0000-000000000001");
        public static readonly Guid Op2                 = new("a1600000-0000-0000-0000-000000000002");
        public static readonly Guid Op3                 = new("a1600000-0000-0000-0000-000000000003");
        public static readonly Guid Op4                 = new("a1600000-0000-0000-0000-000000000004");
        public static readonly Guid Op5                 = new("a1600000-0000-0000-0000-000000000005");
        public static readonly Guid Op6                 = new("a1600000-0000-0000-0000-000000000006");
    }

    private static class DocTypes
    {
        public static readonly Guid Licenses  = new("b1000000-0000-0000-0000-000000000001");
        public static readonly Guid Contracts = new("b1000000-0000-0000-0000-000000000002");
        public static readonly Guid Requisites = new("b1000000-0000-0000-0000-000000000003");
    }

    private static class Docs
    {
        public static readonly Guid License1  = new("b2000000-0000-0000-0000-000000000001");
        public static readonly Guid License2  = new("b2000000-0000-0000-0000-000000000002");
        public static readonly Guid Requisites = new("b2000000-0000-0000-0000-000000000003");
    }

    private static class Cats
    {
        public static readonly Guid Cosmetology  = new("c1000000-0000-0000-0000-000000000001");
        public static readonly Guid Dermatology  = new("c1000000-0000-0000-0000-000000000002");
        public static readonly Guid Lor          = new("c1000000-0000-0000-0000-000000000003");
        public static readonly Guid Plastic      = new("c1000000-0000-0000-0000-000000000004");
        public static readonly Guid Gynecology   = new("c1000000-0000-0000-0000-000000000005");
        public static readonly Guid Massage      = new("c1000000-0000-0000-0000-000000000006");
    }

    private static class Services
    {
        public static readonly Guid Cosmo1 = new("c2000000-0000-0000-0000-000000000001");
        public static readonly Guid Cosmo2 = new("c2000000-0000-0000-0000-000000000002");
        public static readonly Guid Cosmo3 = new("c2000000-0000-0000-0000-000000000003");
        public static readonly Guid Derm1  = new("c2000000-0000-0000-0000-000000000004");
        public static readonly Guid Derm2  = new("c2000000-0000-0000-0000-000000000005");
        public static readonly Guid Lor1   = new("c2000000-0000-0000-0000-000000000006");
        public static readonly Guid Lor2   = new("c2000000-0000-0000-0000-000000000007");
        public static readonly Guid Plas1  = new("c2000000-0000-0000-0000-000000000008");
        public static readonly Guid Plas2  = new("c2000000-0000-0000-0000-000000000009");
        public static readonly Guid Plas3  = new("c2000000-0000-0000-0000-00000000000a");
        public static readonly Guid Gyn1   = new("c2000000-0000-0000-0000-00000000000b");
        public static readonly Guid Mass1  = new("c2000000-0000-0000-0000-00000000000c");
    }

    private static class Specialists
    {
        public static readonly Guid S1 = new("d1000000-0000-0000-0000-000000000001");
        public static readonly Guid S2 = new("d1000000-0000-0000-0000-000000000002");
        public static readonly Guid S3 = new("d1000000-0000-0000-0000-000000000003");
        public static readonly Guid S4 = new("d1000000-0000-0000-0000-000000000004");
    }

    private static class Qualifications
    {
        public static readonly Guid Q1 = new("e1000000-0000-0000-0000-000000000001");
        public static readonly Guid Q2 = new("e1000000-0000-0000-0000-000000000002");
        public static readonly Guid Q3 = new("e1000000-0000-0000-0000-000000000003");
        public static readonly Guid Q4 = new("e1000000-0000-0000-0000-000000000004");
        public static readonly Guid Q5 = new("e1000000-0000-0000-0000-000000000005");
    }

    private static class Accreditations
    {
        public static readonly Guid A1 = new("e2000000-0000-0000-0000-000000000001");
        public static readonly Guid A2 = new("e2000000-0000-0000-0000-000000000002");
        public static readonly Guid A3 = new("e2000000-0000-0000-0000-000000000003");
    }

    private static class Projects
    {
        public static readonly Guid P1 = new("f1000000-0000-0000-0000-000000000001");
        public static readonly Guid P2 = new("f1000000-0000-0000-0000-000000000002");
        public static readonly Guid P3 = new("f1000000-0000-0000-0000-000000000003");
    }

    private static class Slides
    {
        public static readonly Guid Sl1 = new("f2000000-0000-0000-0000-000000000001");
        public static readonly Guid Sl2 = new("f2000000-0000-0000-0000-000000000002");
        public static readonly Guid Sl3 = new("f2000000-0000-0000-0000-000000000003");
    }

    private static class Formats
    {
        public static readonly Guid F1 = new("f3000000-0000-0000-0000-000000000001");
        public static readonly Guid F2 = new("f3000000-0000-0000-0000-000000000002");
        public static readonly Guid F3 = new("f3000000-0000-0000-0000-000000000003");
    }

    private static class Partners
    {
        public static readonly Guid Pt1 = new("f4000000-0000-0000-0000-000000000001");
        public static readonly Guid Pt2 = new("f4000000-0000-0000-0000-000000000002");
        public static readonly Guid Pt3 = new("f4000000-0000-0000-0000-000000000003");
        public static readonly Guid Pt4 = new("f4000000-0000-0000-0000-000000000004");
    }

    private static class OpResults
    {
        public static readonly Guid R1 = new("f5000000-0000-0000-0000-000000000001");
        public static readonly Guid R2 = new("f5000000-0000-0000-0000-000000000002");
        public static readonly Guid R3 = new("f5000000-0000-0000-0000-000000000003");
        public static readonly Guid R4 = new("f5000000-0000-0000-0000-000000000004");
        public static readonly Guid R5 = new("f5000000-0000-0000-0000-000000000005");
        public static readonly Guid R6 = new("f5000000-0000-0000-0000-000000000006");
    }

    private static class NewsIds
    {
        public static readonly Guid N1 = new("f6000000-0000-0000-0000-000000000001");
        public static readonly Guid N2 = new("f6000000-0000-0000-0000-000000000002");
        public static readonly Guid N3 = new("f6000000-0000-0000-0000-000000000003");
        public static readonly Guid N4 = new("f6000000-0000-0000-0000-000000000004");
    }

    private static class Promotions
    {
        public static readonly Guid P1 = new("f7000000-0000-0000-0000-000000000001");
        public static readonly Guid P2 = new("f7000000-0000-0000-0000-000000000002");
        public static readonly Guid P3 = new("f7000000-0000-0000-0000-000000000003");
    }

    private static class Cases
    {
        public static readonly Guid C1 = new("f8000000-0000-0000-0000-000000000001");
        public static readonly Guid C2 = new("f8000000-0000-0000-0000-000000000002");
        public static readonly Guid C3 = new("f8000000-0000-0000-0000-000000000003");
        public static readonly Guid C4 = new("f8000000-0000-0000-0000-000000000004");
    }

    private static class Faqs
    {
        public static readonly Guid F1 = new("f9000000-0000-0000-0000-000000000001");
        public static readonly Guid F2 = new("f9000000-0000-0000-0000-000000000002");
        public static readonly Guid F3 = new("f9000000-0000-0000-0000-000000000003");
        public static readonly Guid F4 = new("f9000000-0000-0000-0000-000000000004");
        public static readonly Guid F5 = new("f9000000-0000-0000-0000-000000000005");
        public static readonly Guid F6 = new("f9000000-0000-0000-0000-000000000006");
    }

    // ====================================================================
    //  ХЕЛПЕРЫ
    // ====================================================================

    private static DateTime Utc(int year, int month, int day, int hour = 0, int minute = 0) =>
        new(year, month, day, hour, minute, 0, DateTimeKind.Utc);

    private static string Img(string slug, int w = 800, int h = 600) =>
        $"https://placehold.co/{w}x{h}/eef2f7/222?text={Uri.EscapeDataString(slug)}";

    // ====================================================================
    //  ТОЧКА ВХОДА
    // ====================================================================

    public static async Task SeedAsync(ApplicationDbContext db, CancellationToken ct = default)
    {
        await SeedMediaFilesAsync(db, ct);
        await SeedDocumentTypesAsync(db, ct);
        await SeedDocumentsAsync(db, ct);
        await SeedServiceCategoriesAsync(db, ct);
        await SeedServicesAsync(db, ct);
        await SeedSpecialistsAsync(db, ct);
        await SeedQualificationsAsync(db, ct);
        await SeedAccreditationsAsync(db, ct);
        await SeedProjectsAsync(db, ct);
        await SeedProjectSlideInfosAsync(db, ct);
        await SeedProjectFormatsAsync(db, ct);
        await SeedProjectPartnersAsync(db, ct);
        await SeedOperationResultsAsync(db, ct);
        await SeedNewsAsync(db, ct);
        await SeedPromotionsAsync(db, ct);
        await SeedCasesAsync(db, ct);
        await SeedFaqsAsync(db, ct);
    }

    // ====================================================================
    //  MEDIA FILES
    // ====================================================================

    private static async Task SeedMediaFilesAsync(ApplicationDbContext db, CancellationToken ct)
    {
        if (await db.MediaFiles.AnyAsync(ct)) return;

        db.MediaFiles.AddRange(
            new MediaFile { Id = Media.CatCosmetology, Name = "Категория: Косметология", Url = Img("Косметология") },
            new MediaFile { Id = Media.CatDermatology, Name = "Категория: Дерматология", Url = Img("Дерматология") },
            new MediaFile { Id = Media.CatLor,         Name = "Категория: ЛОР",         Url = Img("ЛОР") },
            new MediaFile { Id = Media.CatPlastic,     Name = "Категория: Пластика",    Url = Img("Пластика") },
            new MediaFile { Id = Media.CatGynecology,  Name = "Категория: Гинекология", Url = Img("Гинекология") },
            new MediaFile { Id = Media.CatMassage,     Name = "Категория: Массаж",      Url = Img("Массаж") },

            new MediaFile { Id = Media.Spec1Photo, Name = "Фото специалиста 1", Url = Img("Специалист 1", 400, 500) },
            new MediaFile { Id = Media.Spec1Video, Name = "Видео специалиста 1", Url = Img("Видео 1", 800, 450) },
            new MediaFile { Id = Media.Spec2Photo, Name = "Фото специалиста 2", Url = Img("Специалист 2", 400, 500) },
            new MediaFile { Id = Media.Spec2Video, Name = "Видео специалиста 2", Url = Img("Видео 2", 800, 450) },
            new MediaFile { Id = Media.Spec3Photo, Name = "Фото специалиста 3", Url = Img("Специалист 3", 400, 500) },
            new MediaFile { Id = Media.Spec3Video, Name = "Видео специалиста 3", Url = Img("Видео 3", 800, 450) },
            new MediaFile { Id = Media.Spec4Photo, Name = "Фото специалиста 4", Url = Img("Специалист 4", 400, 500) },
            new MediaFile { Id = Media.Spec4Video, Name = "Видео специалиста 4", Url = Img("Видео 4", 800, 450) },

            new MediaFile { Id = Media.Project1Main,  Name = "Проект 1 — главная",   Url = Img("Красота в тебе", 1200, 700) },
            new MediaFile { Id = Media.Project1Slide, Name = "Проект 1 — слайд",     Url = Img("Красота в тебе — слайд") },
            new MediaFile { Id = Media.Project2Main,  Name = "Проект 2 — главная",   Url = Img("Пластика из Милана", 1200, 700) },
            new MediaFile { Id = Media.Project2Slide, Name = "Проект 2 — слайд",     Url = Img("Пластика из Милана — слайд") },
            new MediaFile { Id = Media.Project3Main,  Name = "Проект 3 — главная",   Url = Img("Блефаропластика 2025", 1200, 700) },
            new MediaFile { Id = Media.Project3Slide, Name = "Проект 3 — слайд",     Url = Img("Блефаропластика 2025 — слайд") },

            new MediaFile { Id = Media.Format1Img, Name = "Формат: Индивидуальное преображение", Url = Img("Индивидуальное преображение") },
            new MediaFile { Id = Media.Format2Img, Name = "Формат: Женский клуб",               Url = Img("Женский клуб") },
            new MediaFile { Id = Media.Format3Img, Name = "Формат: Women баскет-лига",          Url = Img("Women баскет-лига") },

            new MediaFile { Id = Media.News1, Name = "Новость 1", Url = Img("Dr.Gaja", 1200, 700) },
            new MediaFile { Id = Media.News2, Name = "Новость 2", Url = Img("Отзывы пациентов") },
            new MediaFile { Id = Media.News3, Name = "Новость 3", Url = Img("Финал проекта") },
            new MediaFile { Id = Media.News4, Name = "Новость 4", Url = Img("Открытие сезона") },

            new MediaFile { Id = Media.Promo1, Name = "Акция 1 — Лепесток",   Url = Img("Нитевой лифтинг Лепесток", 1200, 700) },
            new MediaFile { Id = Media.Promo2, Name = "Акция 2 — PURE MOLECULE", Url = Img("PURE MOLECULE", 1200, 700) },
            new MediaFile { Id = Media.Promo3, Name = "Акция 3 — Чистка лица",  Url = Img("Чистка лица", 1200, 700) },

            new MediaFile { Id = Media.Op1, Name = "Результат операции 1", Url = Img("Результат 1") },
            new MediaFile { Id = Media.Op2, Name = "Результат операции 2", Url = Img("Результат 2") },
            new MediaFile { Id = Media.Op3, Name = "Результат операции 3", Url = Img("Результат 3") },
            new MediaFile { Id = Media.Op4, Name = "Результат операции 4", Url = Img("Результат 4") },
            new MediaFile { Id = Media.Op5, Name = "Результат операции 5", Url = Img("Результат 5") },
            new MediaFile { Id = Media.Op6, Name = "Результат операции 6", Url = Img("Результат 6") }
        );

        await db.SaveChangesAsync(ct);
    }

    // ====================================================================
    //  DOCUMENT TYPES
    // ====================================================================

    private static async Task SeedDocumentTypesAsync(ApplicationDbContext db, CancellationToken ct)
    {
        if (await db.DocumentTypes.AnyAsync(ct)) return;

        db.DocumentTypes.AddRange(
            new DocumentType { Id = DocTypes.Licenses,   Name = "Лицензии и сертификаты" },
            new DocumentType { Id = DocTypes.Contracts,  Name = "Договоры" },
            new DocumentType { Id = DocTypes.Requisites, Name = "Реквизиты" }
        );

        await db.SaveChangesAsync(ct);
    }

    // ====================================================================
    //  DOCUMENTS
    // ====================================================================

    private static async Task SeedDocumentsAsync(ApplicationDbContext db, CancellationToken ct)
    {
        if (await db.Documents.AnyAsync(ct)) return;

        db.Documents.AddRange(
            new Document
            {
                Id = Docs.License1,
                Name = "Лицензия на медицинскую деятельность",
                TypeId = DocTypes.Licenses,
                Url = "/documents/seed/license-medical.pdf"
            },
            new Document
            {
                Id = Docs.License2,
                Name = "Санитарно-эпидемиологическое заключение",
                TypeId = DocTypes.Licenses,
                Url = "/documents/seed/sanitary-certificate.pdf"
            },
            new Document
            {
                Id = Docs.Requisites,
                Name = "Карточка предприятия",
                TypeId = DocTypes.Requisites,
                Url = "/documents/seed/requisites.pdf"
            }
        );

        await db.SaveChangesAsync(ct);
    }

    // ====================================================================
    //  SERVICE CATEGORIES
    // ====================================================================

    private static async Task SeedServiceCategoriesAsync(ApplicationDbContext db, CancellationToken ct)
    {
        if (await db.ServiceCategories.AnyAsync(ct)) return;

        db.ServiceCategories.AddRange(
            new ServiceCategory { Id = Cats.Cosmetology, Title = "Косметология",              ImgUrlId = Media.CatCosmetology },
            new ServiceCategory { Id = Cats.Dermatology, Title = "Дерматология",              ImgUrlId = Media.CatDermatology },
            new ServiceCategory { Id = Cats.Lor,         Title = "Оториноларингология",       ImgUrlId = Media.CatLor },
            new ServiceCategory { Id = Cats.Plastic,     Title = "Пластическая хирургия",     ImgUrlId = Media.CatPlastic },
            new ServiceCategory { Id = Cats.Gynecology,  Title = "Эстетическая гинекология",  ImgUrlId = Media.CatGynecology },
            new ServiceCategory { Id = Cats.Massage,     Title = "Массаж",                    ImgUrlId = Media.CatMassage }
        );

        await db.SaveChangesAsync(ct);
    }

    // ====================================================================
    //  SERVICES
    // ====================================================================

    private static async Task SeedServicesAsync(ApplicationDbContext db, CancellationToken ct)
    {
        if (await db.Services.AnyAsync(ct)) return;

        db.Services.AddRange(
            new Service
            {
                Id = Services.Cosmo1,
                CategoryId = Cats.Cosmetology,
                NomenclatureCode = "A14.01.012",
                MISCode = "3.1.1",
                Duration = 60,
                Price = 4500m,
                DisplayTitle = "Чистка лица ультразвуковая",
                TechnicalTitle = "Ультразвуковая чистка лица",
                Description = "Глубокое очищение кожи с помощью ультразвука.",
                Preparation = "Не наносить макияж за 3 часа до процедуры.",
                Types = "Ультразвуковая,Комбинированная"
            },
            new Service
            {
                Id = Services.Cosmo2,
                CategoryId = Cats.Cosmetology,
                NomenclatureCode = "A14.01.013",
                MISCode = "3.1.2",
                Duration = 90,
                Price = 12000m,
                DisplayTitle = "Нитевой лифтинг «Лепесток»",
                TechnicalTitle = "Нитевой лифтинг лица",
                Preparation = "",
                Description = "Армирование лица рассасывающимися нитями.",
                Types = "Лепесток,Мезонити"
            },
            new Service
            {
                Id = Services.Cosmo3,
                CategoryId = Cats.Cosmetology,
                NomenclatureCode = "A14.01.014",
                MISCode = "3.1.3",
                Duration = 45,
                Price = 8900m,
                DisplayTitle = "Биоревитализация",
                TechnicalTitle = "Инъекционная биоревитализация",
                Preparation = "",
                Description = "Введение гиалуроновой кислоты для увлажнения кожи.",
                Types = "Ультразвуковая,Комбинированная"
            },
            new Service
            {
                Id = Services.Derm1,
                CategoryId = Cats.Dermatology,
                NomenclatureCode = "A12.01.001",
                MISCode = "3.2.1",
                Duration = 30,
                Price = 2200m,
                DisplayTitle = "Консультация дерматолога",
                TechnicalTitle = "Первичный приём врача-дерматолога",
                Preparation = "",
                Description = "Осмотр, сбор анамнеза, назначение лечения.",
                Types = "Ультразвуковая,Комбинированная"
            },
            new Service
            {
                Id = Services.Derm2,
                CategoryId = Cats.Dermatology,
                NomenclatureCode = "A12.01.002",
                MISCode = "3.2.2",
                Duration = 40,
                Price = 3500m,
                DisplayTitle = "Дерматоскопия",
                TechnicalTitle = "Дерматоскопическое исследование новообразований",
                Preparation = "",
                Description = "Исследование родинок и новообразований кожи.",
                Types = "Ультразвуковая,Комбинированная"
            },
            new Service
            {
                Id = Services.Lor1,
                CategoryId = Cats.Lor,
                NomenclatureCode = "A12.08.001",
                MISCode = "3.3.1",
                Duration = 30,
                Price = 2000m,
                DisplayTitle = "Консультация ЛОР-врача",
                TechnicalTitle = "Первичный приём врача-оториноларинголога",
                Description = "Осмотр ЛОР-органов.",
                Preparation = "",
                Types = "Ультразвуковая,Комбинированная"
            },
            new Service
            {
                Id = Services.Lor2,
                CategoryId = Cats.Lor,
                NomenclatureCode = "A16.08.015",
                MISCode = "3.3.2",
                Duration = 60,
                Price = 15000m,
                DisplayTitle = "Септопластика",
                TechnicalTitle = "Хирургическое исправление носовой перегородки",
                Description = "Операция по исправлению носовой перегородки.",
                Preparation = "Сдать анализы, консультация анестезиолога.",
                Types = "Ультразвуковая,Комбинированная"
            },
            new Service
            {
                Id = Services.Plas1,
                CategoryId = Cats.Plastic,
                NomenclatureCode = "A16.20.085",
                MISCode = "3.4.1",
                Duration = 120,
                Price = 112000m,
                DisplayTitle = "Маммопластика (без имплантов)",
                TechnicalTitle = "Увеличение молочной железы",
                Preparation = "",
                Description = "Операция по увеличению груди, без стоимости имплантов.",
                Types = "Первая,Вторая,Третья"
            },
            new Service
            {
                Id = Services.Plas2,
                CategoryId = Cats.Plastic,
                NomenclatureCode = "A16.20.086",
                MISCode = "3.4.2",
                Duration = 120,
                Price = 130000m,
                DisplayTitle = "Мастопексия вертикальная",
                TechnicalTitle = "Подтяжка молочной железы",
                Preparation = "",
                Description = "Подтяжка груди по вертикальной методике."
                ,
                Types = "Ультразвуковая,Комбинированная"
            },
            new Service
            {
                Id = Services.Plas3,
                CategoryId = Cats.Plastic,
                NomenclatureCode = "A16.20.087",
                MISCode = "3.4.3",
                Duration = 150,
                Price = 140000m,
                DisplayTitle = "Абдоминопластика",
                TechnicalTitle = "Пластика живота",
                Preparation = "",
                Description = "Удаление излишков кожи и жира в области живота."
                ,
                Types = "Ультразвуковая,Комбинированная"
            },
            new Service
            {
                Id = Services.Gyn1,
                CategoryId = Cats.Gynecology,
                NomenclatureCode = "A16.21.001",
                MISCode = "3.5.1",
                Duration = 60,
                Price = 8500m,
                DisplayTitle = "Лазерное омоложение интимной зоны",
                TechnicalTitle = "Fotona IntimaLase",
                Preparation = "",
                Description = "Неинвазивная лазерная процедура."
                ,
                Types = "Ультразвуковая,Комбинированная"
            },
            new Service
            {
                Id = Services.Mass1,
                CategoryId = Cats.Massage,
                NomenclatureCode = "A21.01.001",
                MISCode = "3.6.1",
                Duration = 60,
                Price = 3500m,
                DisplayTitle = "Массаж спины",
                TechnicalTitle = "Лечебный массаж спины",
                Preparation = "",
                Description = "Классический массаж спины, 60 минут."
                ,
                Types = "Ультразвуковая,Комбинированная"
            }
        );

        await db.SaveChangesAsync(ct);
    }

    // ====================================================================
    //  SPECIALISTS
    // ====================================================================

    private static async Task SeedSpecialistsAsync(ApplicationDbContext db, CancellationToken ct)
    {
        if (await db.Specialists.AnyAsync(ct)) return;

        db.Specialists.AddRange(
            new Specialist
            {
                Id = Specialists.S1,
                FirstName = "Ирина",
                MidName = "Сергеевна",
                LastName = "Кузнецова",
                Department = "Руководство",
                Description = "Директор клиники, врач-дерматовенеролог, косметолог. Стаж более 20 лет.",
                ImgId = Media.Spec1Photo,
                VideoId = Media.Spec1Video,
                StartDate = Utc(2005, 9, 1),
                OperationsCount = 320,
                PositiveReviewsPercentage = 98,
                ActivityArea = new List<string> { "Дерматология", "Косметология", "Трихология" }
            },
            new Specialist
            {
                Id = Specialists.S2,
                FirstName = "Юрий",
                MidName = "Юрьевич",
                LastName = "Перевезенцев",
                Department = "Хирургия",
                Description = "Пластический хирург, кандидат медицинских наук, врач высшей категории.",
                ImgId = Media.Spec2Photo,
                VideoId = Media.Spec2Video,
                StartDate = Utc(2010, 3, 15),
                OperationsCount = 540,
                PositiveReviewsPercentage = 97,
                ActivityArea = new List<string> { "Маммопластика", "Абдоминопластика", "Блефаропластика" }
            },
            new Specialist
            {
                Id = Specialists.S3,
                FirstName = "Евгения",
                MidName = "Александровна",
                LastName = "Сергеева",
                Department = "Оториноларингология",
                Description = "Врач-оториноларинголог, фониатр, ведущий специалист по лечению синуситов.",
                ImgId = Media.Spec3Photo,
                VideoId = Media.Spec3Video,
                StartDate = Utc(2002, 8, 1),
                OperationsCount = 210,
                PositiveReviewsPercentage = 95,
                ActivityArea = new List<string> { "Оториноларингология", "Фониатрия" }
            },
            new Specialist
            {
                Id = Specialists.S4,
                FirstName = "Анна",
                MidName = "Петровна",
                LastName = "Бусыгина",
                Department = "Косметология",
                Description = "Врач-дерматовенеролог, косметолог, трихолог.",
                ImgId = Media.Spec4Photo,
                VideoId = Media.Spec4Video,
                StartDate = Utc(2015, 6, 1),
                OperationsCount = 180,
                PositiveReviewsPercentage = 96,
                ActivityArea = new List<string> { "Косметология", "Трихология" }
            }
        );

        await db.SaveChangesAsync(ct);
    }

    // ====================================================================
    //  QUALIFICATIONS
    // ====================================================================

    private static async Task SeedQualificationsAsync(ApplicationDbContext db, CancellationToken ct)
    {
        if (await db.Qualifications.AnyAsync(ct)) return;

        db.Qualifications.AddRange(
            new Qualification
            {
                Id = Qualifications.Q1,
                SpecialistId = Specialists.S1,
                Title = "Врач-дерматовенеролог",
                Level = "Высшее образование",
                Organization = "Челябинская государственная медицинская академия",
                IssuanceDate = Utc(2005, 6, 20)
            },
            new Qualification
            {
                Id = Qualifications.Q2,
                SpecialistId = Specialists.S2,
                Title = "Врач-хирург",
                Level = "Высшее образование",
                Organization = "Южно-Уральский государственный медицинский университет",
                IssuanceDate = Utc(2010, 6, 15)
            },
            new Qualification
            {
                Id = Qualifications.Q3,
                SpecialistId = Specialists.S2,
                Title = "Пластическая хирургия",
                Level = "Ординатура",
                Organization = "ЮУГМУ",
                IssuanceDate = Utc(2012, 6, 30)
            },
            new Qualification
            {
                Id = Qualifications.Q4,
                SpecialistId = Specialists.S3,
                Title = "Врач-педиатр",
                Level = "Высшее образование",
                Organization = "ЧелГМА",
                IssuanceDate = Utc(2002, 6, 25)
            },
            new Qualification
            {
                Id = Qualifications.Q5,
                SpecialistId = Specialists.S4,
                Title = "Врач-дерматовенеролог",
                Level = "Высшее образование",
                Organization = "ЮУГМУ",
                IssuanceDate = Utc(2015, 6, 20)
            }
        );

        await db.SaveChangesAsync(ct);
    }

    // ====================================================================
    //  ACCREDITATIONS
    // ====================================================================

    private static async Task SeedAccreditationsAsync(ApplicationDbContext db, CancellationToken ct)
    {
        if (await db.Accreditations.AnyAsync(ct)) return;

        db.Accreditations.AddRange(
            new Accreditation
            {
                Id = Accreditations.A1,
                SpecialistId = Specialists.S2,
                DocumentType = "Сертификат",
                Specialisation = "Пластическая хирургия",
                JobTitle = "Врач-пластический хирург",
                IssuanceDate = Utc(2023, 3, 15),
                ValidityPeriod = Utc(2028, 3, 15),
                Description = "Действует до 15.03.2028"
            },
            new Accreditation
            {
                Id = Accreditations.A2,
                SpecialistId = Specialists.S3,
                DocumentType = "Сертификат",
                Specialisation = "Оториноларингология",
                JobTitle = "Врач-оториноларинголог",
                IssuanceDate = Utc(2022, 11, 17),
                ValidityPeriod = Utc(2027, 11, 17),
                Description = "Пролонгирован по приказу до 31.12.2028"
            },
            new Accreditation
            {
                Id = Accreditations.A3,
                SpecialistId = Specialists.S1,
                DocumentType = "Сертификат",
                Specialisation = "Дерматовенерология",
                JobTitle = "Врач-дерматовенеролог",
                IssuanceDate = Utc(2021, 5, 20),
                ValidityPeriod = Utc(2026, 5, 20),
    Description = "Действует до 20.05.2026"  
            }
        );

        await db.SaveChangesAsync(ct);
    }

    // ====================================================================
    //  PROJECTS
    // ====================================================================

    private static async Task SeedProjectsAsync(ApplicationDbContext db, CancellationToken ct)
    {
        if (await db.Projects.AnyAsync(ct)) return;

        db.Projects.AddRange(
            new Project
            {
                Id = Projects.P1,
                Title = "Социальный проект «Красота в тебе»",
                MainImgUrlId = Media.Project1Main,
                FirstDescription = "«Красота в тебе» — социальный проект для жителей Челябинской области, которые хотят преобразиться, доверив своё преображение лучшим специалистам.",
                SecondDescription = "Проект имеет международный статус — в нём участвует итальянский пластический хирург Dr. Ivan Gaja.",
                Date = Utc(2025, 4, 7),
                SeasonCount = 11,
                MemberCount = 5026,
                ExpertCount = 35,
                PartnerCount = 67,
                Stages = new string[]
                {
                    "Оффлайн-кастинг",
                    "Пресс-конференция",
                    "Работа с психологом",
                    "Преображение у пластического хирурга",
                    "Красивая медицина от Арт-Медика",
                    "Красивая улыбка от стоматологии",
                    "Преображение со стилистом",
                    "Мастер-классы от экспертов и партнеров",
                    "Встречи женского клуба «Красота в тебе»",
                    "Финал проекта"
                }
            },
            new Project
            {
                Id = Projects.P2,
                Title = "Доступная пластическая хирургия из Милана в России",
                MainImgUrlId = Media.Project2Main,
                FirstDescription = "Российско-Итальянский проект, в рамках которого сертифицированный пластический хирург Dr. Gaja внедряет авторские методики.",
                SecondDescription = "Доступная пластическая хирургия европейского уровня для пациентов из России.",
                Date = Utc(2025, 3, 20),
                SeasonCount = 7,
                MemberCount = 1200,
                ExpertCount = 12,
                PartnerCount = 20,
                Stages = new string[]
                {
                    "Консультация с Dr. Gaja",
                    "Подготовка к операции",
                    "Операция",
                    "Реабилитация"
                }
            },
            new Project
            {
                Id = Projects.P3,
                Title = "Европейская блефаро-пластика 2025",
                MainImgUrlId = Media.Project3Main,
                FirstDescription = "Комплексная программа омоложения верхнего века с хирургической пластикой и курсом реабилитации на лазерной системе Fotona.",
                SecondDescription = "Сочетание хирургии и лазерных технологий для естественного результата.",
                Date = Utc(2025, 2, 1),
                SeasonCount = 3,
                MemberCount = 250,
                ExpertCount = 6,
                PartnerCount = 9,
                Stages = new string[]
                {
                    "Консультация хирурга",
                    "Операция",
                    "3 сеанса Fotona",
                    "Контрольный осмотр"
                }
            }
        );

        await db.SaveChangesAsync(ct);
    }

    // ====================================================================
    //  PROJECT SLIDES
    // ====================================================================

    private static async Task SeedProjectSlideInfosAsync(ApplicationDbContext db, CancellationToken ct)
    {
        if (await db.ProjectSlideInfos.AnyAsync(ct)) return;

        db.ProjectSlideInfos.AddRange(
            new ProjectSlideInfo
            {
                Id = Slides.Sl1,
                ProjectId = Projects.P1,
                ImgUrlId = Media.Project1Slide,
                Description = "Новый сезон уже стартовал — успейте подать заявку",
                Date = Utc(2025, 4, 7)
            },
            new ProjectSlideInfo
            {
                Id = Slides.Sl2,
                ProjectId = Projects.P2,
                ImgUrlId = Media.Project2Slide,
                Description = "Пластика европейского уровня в Челябинске",
                Date = Utc(2025, 3, 20)
            },
            new ProjectSlideInfo
            {
                Id = Slides.Sl3,
                ProjectId = Projects.P3,
                ImgUrlId = Media.Project3Slide,
                Description = "Омоложение без следов — естественный результат",
                Date = Utc(2025, 2, 1)
            }
        );

        await db.SaveChangesAsync(ct);
    }

    // ====================================================================
    //  PROJECT FORMATS
    // ====================================================================

    private static async Task SeedProjectFormatsAsync(ApplicationDbContext db, CancellationToken ct)
    {
        if (await db.ProjectFormats.AnyAsync(ct)) return;

        db.ProjectFormats.AddRange(
            new ProjectFormat
            {
                Id = Formats.F1,
                ProjectId = Projects.P1,
                ImgId = Media.Format1Img,
                Title = "Индивидуальное преображение",
                Description = "Отправная точка к новым возможностям, вдохновению и качественным переменам в жизни."
            },
            new ProjectFormat
            {
                Id = Formats.F2,
                ProjectId = Projects.P1,
                ImgId = Media.Format2Img,
                Title = "Женский клуб «Красота в тебе»",
                Description = "Сообщество, открывающее женщинам широкие возможности для мотивации, вдохновения и самореализации."
            },
            new ProjectFormat
            {
                Id = Formats.F3,
                ProjectId = Projects.P1,
                ImgId = Media.Format3Img,
                Title = "Women баскет-лига",
                Description = "Резиденты клуба объединяются в команды и играют в любительский баскетбол 3x3."
            }
        );

        await db.SaveChangesAsync(ct);
    }

    // ====================================================================
    //  PROJECT PARTNERS
    // ====================================================================

    private static async Task SeedProjectPartnersAsync(ApplicationDbContext db, CancellationToken ct)
    {
        if (await db.ProjectPartners.AnyAsync(ct)) return;

        db.ProjectPartners.AddRange(
            new ProjectPartner
            {
                Id = Partners.Pt1,
                ProjectId = Projects.P1,
                Title = "Dr. Gaja",
                Description = "Итальянский пластический хирург"
            },
            new ProjectPartner
            {
                Id = Partners.Pt2,
                ProjectId = Projects.P1,
                Title = "Арт-Медика",
                Description = "Медицинский центр"
            },
            new ProjectPartner
            {
                Id = Partners.Pt3,
                ProjectId = Projects.P1,
                Title = "Гранд Успех",
                Description = "Стоматологическая клиника"
            },
            new ProjectPartner
            {
                Id = Partners.Pt4,
                ProjectId = Projects.P2,
                Title = "Dr. Gaja",
                Description = "Пластический хирург, Милан"
            }
        );

        await db.SaveChangesAsync(ct);
    }

    // ====================================================================
    //  OPERATION RESULTS
    // ====================================================================

    private static async Task SeedOperationResultsAsync(ApplicationDbContext db, CancellationToken ct)
    {
        if (await db.OperationResults.AnyAsync(ct)) return;

        db.OperationResults.AddRange(
            new OperationResult { Id = OpResults.R1, ImgUrlId = Media.Op1, OperationId = Services.Plas1, SpecialistId = Specialists.S2 },
            new OperationResult { Id = OpResults.R2, ImgUrlId = Media.Op2, OperationId = Services.Plas1, SpecialistId = Specialists.S2 },
            new OperationResult { Id = OpResults.R3, ImgUrlId = Media.Op3, OperationId = Services.Plas2, SpecialistId = Specialists.S2 },
            new OperationResult { Id = OpResults.R4, ImgUrlId = Media.Op4, OperationId = Services.Plas3, SpecialistId = Specialists.S2 },
            new OperationResult { Id = OpResults.R5, ImgUrlId = Media.Op5, OperationId = Services.Cosmo2, SpecialistId = Specialists.S1 },
            new OperationResult { Id = OpResults.R6, ImgUrlId = Media.Op6, OperationId = Services.Cosmo1, SpecialistId = Specialists.S4 }
        );

        await db.SaveChangesAsync(ct);
    }

    // ====================================================================
    //  NEWS
    // ====================================================================

    private static async Task SeedNewsAsync(ApplicationDbContext db, CancellationToken ct)
    {
        if (await db.News.AnyAsync(ct)) return;

        db.News.AddRange(
            new New
            {
                Id = NewsIds.N1,
                Title = "«Освежиться, но не более!» Пластический хирург Dr.Gaja о бьюти-трендах и операциях",
                ImgUrlId = Media.News1,
                Description = "В гостях у «Арт-Медика» — известный пластический хирург Dr.Gaja. Он рассказал, какие запросы пациентов уходят в прошлое, а на смену чему приходит вечная классика.\n\nГлавный тезис встречи: «Освежиться, но не более». По мнению Dr.Gaja, эпоха кардинальных преображений уходит."
            },
            new New
            {
                Id = NewsIds.N2,
                Title = "Отзывы пациентов – наша гордость!",
                ImgUrlId = Media.News2,
                Description = "Рейтинг клиники «Арт-Медика» на портале «ПроДокторов» достиг 4.5 на основе 118 отзывов. Спасибо за доверие!"
            },
            new New
            {
                Id = NewsIds.N3,
                Title = "Финал 4 сезона международного проекта «Красота в тебе»",
                ImgUrlId = Media.News3,
                Description = "Состоялся финал очередного сезона социального проекта «Красота в тебе». Участницы представили свои результаты на сцене."
            },
            new New
            {
                Id = NewsIds.N4,
                Title = "Открытие нового сезона проекта «Красота в тебе»",
                ImgUrlId = Media.News4,
                Description = "Мы рады объявить о старте заявок на новый сезон проекта «Красота в тебе»."
            }
        );

        await db.SaveChangesAsync(ct);
    }

    // ====================================================================
    //  PROMOTIONS
    // ====================================================================

    private static async Task SeedPromotionsAsync(ApplicationDbContext db, CancellationToken ct)
    {
        if (await db.Promotions.AnyAsync(ct)) return;

        db.Promotions.AddRange(
            new Promotion
            {
                Id = Promotions.P1,
                Title = "Нитевой лифтинг «Лепесток» — скидка 50%",
                ImgUrlId = Media.Promo1,
                Description = "Освежить лицо, подчеркнуть красоту и вернуть коже упругость. Уникальная методика «Лепесток» позволяет эффективно подтянуть среднюю и нижнюю треть лица.",
                Date = Utc(2025, 4, 11)
            },
            new Promotion
            {
                Id = Promotions.P2,
                Title = "PURE MOLECULE — программа комплексного омоложения",
                ImgUrlId = Media.Promo2,
                Description = "Инновационная технология омоложения организма — уникальная разработка европейских врачей-косметологов.",
                Date = Utc(2025, 4, 5)
            },
            new Promotion
            {
                Id = Promotions.P3,
                Title = "Ультразвуковая чистка лица — скидка 20%",
                ImgUrlId = Media.Promo3,
                Description = "Профессиональное очищение кожи с помощью ультразвука. Видимый результат уже после первой процедуры.",
                Date = Utc(2025, 3, 28)
            }
        );

        await db.SaveChangesAsync(ct);
    }

    // ====================================================================
    //  CASES
    // ====================================================================

    private static async Task SeedCasesAsync(ApplicationDbContext db, CancellationToken ct)
    {
        if (await db.Cases.AnyAsync(ct)) return;

        db.Cases.AddRange(
            new Case
            {
                Id = Cases.C1,
                ClientName = "Ольга",
                Review = 5,
                Date = Utc(2025, 4, 7),
                Problem = "Выраженное опущение мягких тканей, потеря чёткого овала лица и признаки усталости.",
                Solution = "Выполнили S-лифтинг лица — малотравматичная операция с быстрым восстановлением."
            },
            new Case
            {
                Id = Cases.C2,
                ClientName = "Марина",
                Review = 5,
                Date = Utc(2025, 3, 20),
                Problem = "Небольшая асимметрия груди после кормления, потеря формы.",
                Solution = "Проведена маммопластика с установкой имплантов."
            },
            new Case
            {
                Id = Cases.C3,
                ClientName = "Елена",
                Review = 5,
                Date = Utc(2025, 3, 5),
                Problem = "Хроническая заложенность носа, искривление перегородки.",
                Solution = "Септопластика — восстановление носового дыхания."
            },
            new Case
            {
                Id = Cases.C4,
                ClientName = "Анна",
                Review = 4,
                Date = Utc(2025, 2, 15),
                Problem = "Тусклый цвет лица, расширенные поры.",
                Solution = "Курс ультразвуковой чистки и биоревитализации."
            }
        );

        await db.SaveChangesAsync(ct);
    }

    // ====================================================================
    //  FAQ
    // ====================================================================

    private static async Task SeedFaqsAsync(ApplicationDbContext db, CancellationToken ct)
    {
        if (await db.Faqs.AnyAsync(ct)) return;

        db.Faqs.AddRange(
            new Faq
            {
                Id = Faqs.F1,
                Question = "Насколько безопасны методы и оборудование?",
                Answer = "Мы используем только сертифицированные технологии и следуем строгим медицинским стандартам."
            },
            new Faq
            {
                Id = Faqs.F2,
                Question = "Сколько времени занимает курс процедур?",
                Answer = "Продолжительность курса зависит от индивидуальных особенностей кожи и выбранной методики. В среднем — от 3 до 6 сеансов с интервалом в 2–4 недели."
            },
            new Faq
            {
                Id = Faqs.F3,
                Question = "Есть ли противопоказания?",
                Answer = "Да, как и у любых медицинских манипуляций, существуют противопоказания: беременность, лактация, острые инфекционные заболевания, онкология. Перед процедурой обязательна консультация врача."
            },
            new Faq
            {
                Id = Faqs.F4,
                Question = "Можно ли сочетать несколько процедур одновременно?",
                Answer = "Многие методики отлично дополняют друг друга. План лечения составляет врач на приёме."
            },
            new Faq
            {
                Id = Faqs.F5,
                Question = "Нужно ли проходить обследование перед процедурой?",
                Answer = "Для большинства косметологических уходов глубокое обследование не требуется, но для сложных инвазивных процедур или пластической хирургии необходим стандартный перечень анализов."
            },
            new Faq
            {
                Id = Faqs.F6,
                Question = "Какие процедуры подходят именно мне?",
                Answer = "Подбор программы осуществляется индивидуально на первичной консультации. Врач проводит диагностику состояния тканей, собирает анамнез и подбирает методики под ваши цели."
            }
        );

        await db.SaveChangesAsync(ct);
    }
}