
using Microsoft.AspNetCore.Hosting;

namespace ArtMedica.WebApi.Services;

public sealed class MediaStorageService
{
    private readonly IWebHostEnvironment _environment;

    private static readonly HashSet<string> ImageExtensions =
        new(StringComparer.OrdinalIgnoreCase)
        {
            ".jpg", ".jpeg", ".png", ".gif",
            ".webp", ".avif", ".svg"
        };

    private static readonly HashSet<string> VideoExtensions =
        new(StringComparer.OrdinalIgnoreCase)
        {
            ".mp4", ".webm", ".mov", ".avi", ".mkv", ".m4v"
        };

    private const long MaxFileSize = 100 * 1024 * 1024;

    public MediaStorageService(IWebHostEnvironment environment)
    {
        _environment = environment;
    }

    public async Task<UploadedMedia> SaveAsync(
        IFormFile file,
        CancellationToken cancellationToken = default)
    {
        if (file.Length <= 0)
            throw new InvalidOperationException("Файл пуст.");

        if (file.Length > MaxFileSize)
            throw new InvalidOperationException(
                "Максимальный размер файла — 100 МБ.");

        var extension = Path.GetExtension(file.FileName);

        string type;

        if (ImageExtensions.Contains(extension))
            type = "image";
        else if (VideoExtensions.Contains(extension))
            type = "video";
        else
            throw new InvalidOperationException(
                $"Формат {extension} не поддерживается.");

        var webRoot = _environment.WebRootPath
            ?? Path.Combine(_environment.ContentRootPath, "wwwroot");

        var directory = Path.Combine(webRoot, "uploads", "media");
        Directory.CreateDirectory(directory);

        var fileName = $"{Guid.NewGuid():N}{extension.ToLowerInvariant()}";
        var path = Path.Combine(directory, fileName);

        await using var stream = new FileStream(
            path,
            FileMode.CreateNew,
            FileAccess.Write,
            FileShare.None,
            81920,
            useAsync: true);

        await file.CopyToAsync(stream, cancellationToken);

        return new UploadedMedia(
            fileName,
            type,
            $"/uploads/media/{fileName}");
    }
}

public sealed record UploadedMedia(
    string FileName,
    string Type,
    string Url);
