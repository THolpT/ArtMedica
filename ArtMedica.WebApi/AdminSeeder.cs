using Microsoft.AspNetCore.Identity;

namespace ArtMedica.Application.Seeding;

/// <summary>
/// Создаёт фиксированного администратора (email/пароль/роль захардкожены).
/// Метод идемпотентный — повторный запуск безопасен.
/// </summary>
public static class AdminSeeder
{
    public const string Role     = "Admin";
    public const string Email    = "admin@artmedica.ru";
    public const string Password = "ArtMedica2025!";

    public static async Task SeedAsync(IServiceProvider services, CancellationToken ct = default)
    {
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = services.GetRequiredService<UserManager<IdentityUser>>();

        if (!await roleManager.RoleExistsAsync(Role))
        {
            await roleManager.CreateAsync(new IdentityRole(Role));
        }

        var user = await userManager.FindByEmailAsync(Email);
        if (user is null)
        {
            user = new IdentityUser
            {
                UserName = Email,       // PasswordSignInAsync использует UserName
                Email = Email,
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(user, Password);
            if (!result.Succeeded)
            {
                var errors = string.Join("; ", result.Errors.Select(e => e.Description));
                throw new InvalidOperationException($"Не удалось создать администратора: {errors}");
            }
        }

        if (!await userManager.IsInRoleAsync(user, Role))
        {
            await userManager.AddToRoleAsync(user, Role);
        }
    }
}