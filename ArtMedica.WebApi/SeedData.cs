using ArtMedica.Application;
using ArtMedica.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

public static class SeedData
{
    public static async Task InitializeAsync(IServiceProvider serviceProvider)
    {
        await SeedIdentityAsync(serviceProvider);
        await SeedBusinessDataAsync(serviceProvider);
    }

    // ------------------------------------------------------------------
    // Identity: роль администратора + сам пользователь
    // ------------------------------------------------------------------
    private static async Task SeedIdentityAsync(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

        const string adminRole = "Administrator";
        const string adminEmail = "admin@artmedica.local";
        const string adminPassword = "admin123"; // ⚠️ смените после первого входа

        if (!await roleManager.RoleExistsAsync(adminRole))
        {
            await roleManager.CreateAsync(new IdentityRole(adminRole));
        }

        var adminUser = await userManager.FindByEmailAsync(adminEmail);
        if (adminUser is null)
        {
            adminUser = new IdentityUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(adminUser, adminPassword);
            if (!result.Succeeded)
            {
                throw new Exception("Не удалось создать администратора: " +
                    string.Join("; ", result.Errors.Select(e => e.Description)));
            }
        }

        if (!await userManager.IsInRoleAsync(adminUser, adminRole))
        {
            await userManager.AddToRoleAsync(adminUser, adminRole);
        }
    }

    // ------------------------------------------------------------------
    // Бизнес-данные: категории, кейсы, акции
    // ------------------------------------------------------------------
    private static async Task SeedBusinessDataAsync(IServiceProvider serviceProvider)
    {
        var db = serviceProvider.GetRequiredService<ApplicationDbContext>();

        await SeedServiceCategoriesAsync(db);
        await SeedCasesAsync(db);
        await SeedPromotionsAsync(db);

        await db.SaveChangesAsync();
    }

    private static async Task SeedServiceCategoriesAsync(ApplicationDbContext db)
    {
        if (await db.ServiceCategories.AnyAsync())
            return;

        db.ServiceCategories.AddRange(
            new ServiceCategory { Title = "Диагностика" },
            new ServiceCategory { Title = "Хирургия" },
            new ServiceCategory { Title = "Терапия" },
            new ServiceCategory { Title = "Реабилитация" }
        );
    }

    private static async Task SeedCasesAsync(ApplicationDbContext db)
    {
        if (await db.Cases.AnyAsync())
            return;

        db.Cases.AddRange(
            new Case
            {
                ClientName = "ООО \"Ромашка\"",
                Problem = "Пациент 45 лет, жалобы на боли в пояснице в течение 3 месяцев, ограничение подвижности.",
                Solution = "Проведён полный курс МРТ-диагностики, назначена комплексная терапия. Через 6 недель — полное восстановление подвижности.",
                Date = new DateTime(2024, 11, 12, 10, 0, 0, DateTimeKind.Utc),
                Review = 5
            },
            new Case
            {
                ClientName = "Иванов И.И.",
                Problem = "Перелом лучевой кости после падения, требуется хирургическое вмешательство.",
                Solution = "Проведена операция с установкой металлоконструкции. Реабилитация 8 недель, полное восстановление функций кисти.",
                Date = new DateTime(2024, 10, 3, 14, 30, 0, DateTimeKind.Utc),
                Review = 5
            },
            new Case
            {
                ClientName = "Сидорова А.П.",
                Problem = "Хроническая мигрень, не поддающаяся стандартному лечению.",
                Solution = "Комплексное обследование, курс ботулинотерапии. Частота приступов снижена с 12 до 2 в месяц.",
                Date = new DateTime(2024, 9, 15, 9, 15, 0, DateTimeKind.Utc),
                Review = 4
            },
            new Case
            {
                ClientName = "Петров П.П.",
                Problem = "Разрыв мениска коленного сустава после спортивной травмы.",
                Solution = "Артроскопическая операция, курс физиотерапии. Возврат к спорту через 4 месяца.",
                Date = new DateTime(2024, 8, 20, 11, 45, 0, DateTimeKind.Utc),
                Review = 5
            },
            new Case
            {
                ClientName = "ООО \"Здоровье+\"",
                Problem = "Плановая диспансеризация сотрудников, 120 человек.",
                Solution = "Организован выездной медосмотр, результаты в течение 5 рабочих дней. Сотрудничество продолжено.",
                Date = new DateTime(2024, 7, 5, 8, 0, 0, DateTimeKind.Utc),
                Review = 5
            }
        );
    }

    private static async Task SeedPromotionsAsync(ApplicationDbContext db)
    {
        if (await db.Promotions.AnyAsync())
            return;

        db.Promotions.AddRange(
            new Promotion
            {
                Title = "Скидка 20% на первичную диагностику",
                Description = "Для новых пациентов — скидка 20% на любое первичное диагностическое обследование до конца месяца.",
                Date = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new Promotion
            {
                Title = "Бесплатная консультация при записи онлайн",
                Description = "При записи через сайт — бесплатная первичная консультация специалиста.",
                Date = new DateTime(2025, 2, 15, 0, 0, 0, DateTimeKind.Utc)
            },
            new Promotion
            {
                Title = "Комплексная реабилитация со скидкой 15%",
                Description = "Курс реабилитации из 10 сеансов — по специальной цене. Действует до конца квартала.",
                Date = new DateTime(2025, 3, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new Promotion
            {
                Title = "Check-up всего организма за 9990 ₽",
                Description = "Полное обследование: анализы, УЗИ, ЭКГ, консультации 3 специалистов. Экономия до 40%.",
                Date = new DateTime(2024, 12, 10, 0, 0, 0, DateTimeKind.Utc)
            }
        );
    }
}