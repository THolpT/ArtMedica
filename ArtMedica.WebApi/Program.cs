using ArtMedica.Application;
using ArtMedica.Application.Seeding;
using ArtMedica.Domain.Models;
using ArtMedica.WebApi.Abstractions;
using ArtMedica.WebApi.Configuration;
using ArtMedica.WebApi.Services;
using DotNetEd.CoreAdmin;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RazorRichText.Extensions;
using Saritasa.NetForge.Extensions;
using Saritasa.NetForge.Infrastructure.EfCore.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<ApplicationDbContext>();

// Cookie-аутентификация: куда редиректить неавторизованных
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.LogoutPath = "/Account/Logout";
    options.AccessDeniedPath = "/Account/Login";
    options.ExpireTimeSpan = TimeSpan.FromHours(8);
    options.SlidingExpiration = true;
    options.Cookie.Name = "ArtMedica.Admin";
    options.Cookie.HttpOnly = true;
    options.Cookie.SameSite = SameSiteMode.Lax;
});

// Политика «только админ» — используется ниже в конвенциях Razor Pages
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy =>
        policy.RequireRole(AdminSeeder.Role));
});

builder.Services.AddScoped<IAdminRegistry, AdminRegistry>();
builder.Services.AddScoped<MediaStorageService>();

builder.Services.AddControllersWithViews();

// Закрываем ВСЕ страницы под /Admin ролью Admin, Login оставляем анонимным
builder.Services.AddRazorPages(options =>
{
    options.Conventions.AuthorizeFolder("/Admin", "AdminOnly");
    options.Conventions.AllowAnonymousToPage("/Account/Login");
});

builder.Services.AddRazorRichText(opts =>
{
    opts.DefaultHeight = 100;
    opts.DefaultPlaceholder = "Введите текст...";
});

builder.Services.AddArtMedicaAdmin();

builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
        policy.WithOrigins("http://localhost:5173", "https://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials());
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("Frontend");

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapDefaultControllerRoute();
app.MapRazorPages();
app.MapRazorRichTextAssets();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

    if (app.Environment.IsDevelopment() || app.Environment.IsStaging())
    {
        await db.Database.MigrateAsync();
        await DbSeeder.SeedAsync(db);
        logger.LogInformation("Database migrated and seeded");
    }

    await AdminSeeder.SeedAsync(scope.ServiceProvider);
    logger.LogInformation("Fixed admin user ensured");
}

app.Run();