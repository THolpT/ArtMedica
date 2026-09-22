using ArtMedica.Application;
using DotNetEd.CoreAdmin;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Saritasa.NetForge.Extensions;
using Saritasa.NetForge.Infrastructure.EfCore.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDefaultIdentity<IdentityUser>(options =>
    {
        options.SignIn.RequireConfirmedAccount = false; // отключаем требование подтверждения — для админа это лишнее
        options.Password.RequireDigit = false;
        options.Password.RequiredLength = 6;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireLowercase = false;
    })
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

// ⬇️ Ключевой момент — переопределяем пути Identity на наши страницы
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.LogoutPath = "/Account/Logout";
    options.AccessDeniedPath = "/Account/Login";
});

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.AddCoreAdmin("Administrator");
builder.Services.AddCoreAdmin(new CoreAdminOptions
{
    IgnoreEntityTypes = new List<Type>
    {
        typeof(IdentityUserRole<string>),
        typeof(IdentityRole),
        typeof(IdentityRoleClaim<string>),
        typeof(IdentityUser),
        typeof(IdentityUserClaim<string>),
        typeof(IdentityUserLogin<string>),
        typeof(IdentityUserToken<string>),
        typeof(IdentityUserPasskey<string>)
    }
});

var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapDefaultControllerRoute();
app.MapRazorPages();

using (var scope = app.Services.CreateScope())
{
    await SeedData.InitializeAsync(scope.ServiceProvider);
}

app.Run();