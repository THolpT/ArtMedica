using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace ArtMedica.WebApi.Pages.Account;

public class LoginModel : PageModel
{
    private const string DefaultReturnUrl = "~/Admin";

    private readonly SignInManager<IdentityUser> _signInManager;

    public LoginModel(SignInManager<IdentityUser> signInManager)
    {
        _signInManager = signInManager;
    }

    [BindProperty]
    public InputModel Input { get; set; } = new();

    public string? ReturnUrl { get; set; }

    [TempData]
    public string? ErrorMessage { get; set; }

    public class InputModel
    {
        [Required(ErrorMessage = "Укажите email")]
        [EmailAddress(ErrorMessage = "Некорректный email")]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Укажите пароль")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; } = string.Empty;

        [Display(Name = "Запомнить меня")]
        public bool RememberMe { get; set; }
    }

    public IActionResult OnGet(string? returnUrl = null)
    {
        var target = ResolveReturnUrl(returnUrl);

        if (User.Identity?.IsAuthenticated == true)
        {
            return LocalRedirect(target);
        }

        if (!string.IsNullOrEmpty(ErrorMessage))
        {
            ModelState.AddModelError(string.Empty, ErrorMessage);
        }

        ReturnUrl = target;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
    {
        // Приоритет: query-строка → свойство из формы → значение по умолчанию
        var target = ResolveReturnUrl(
            Url.IsLocalUrl(returnUrl) ? returnUrl : ReturnUrl);

        if (!ModelState.IsValid)
        {
            ReturnUrl = target;
            return Page();
        }

        var result = await _signInManager.PasswordSignInAsync(
            Input.Email,
            Input.Password,
            Input.RememberMe,
            lockoutOnFailure: false);

        if (result.Succeeded)
        {
            return LocalRedirect(target);
        }

        ModelState.AddModelError(
            string.Empty,
            result.IsLockedOut
                ? "Аккаунт заблокирован."
                : "Неверный email или пароль.");

        ReturnUrl = target;
        return Page();
    }

    private string ResolveReturnUrl(string? candidate)
    {
        if (Url.IsLocalUrl(candidate))
            return candidate!;

        return Url.Content(DefaultReturnUrl);
    }
}