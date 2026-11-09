using Microsoft.AspNetCore.Hosting;

namespace ArtMedica.WebApi.Services;

public sealed class DocumentStorageService
{
    private readonly IWebHostEnvironment _environment;

    public DocumentStorageService(IWebHostEnvironment environment)
    {
        _environment = environment;
    }

    public async Task<UploadedDocument> SaveAsync(
        IFormFile file,
        CancellationToken cancellationToken = default)
    {
        if (file is null || file.Length <= 0)
            throw new InvalidOperationException("Файл пуст.");

        var webRoot = _environment.WebRootPath
            ?? Path.Combine(_environment.ContentRootPath, "wwwroot");

        var directory = Path.Combine(webRoot, "documents");
        Directory.CreateDirectory(directory);

        var extension = Path.GetExtension(file.FileName);
        var fileName = $"{Guid.NewGuid():N}{extension}";
        var path = Path.Combine(directory, fileName);

        await using (var stream = new FileStream(
            path,
            FileMode.CreateNew,
            FileAccess.Write,
            FileShare.None,
            81920,
            useAsync: true))
        {
            await file.CopyToAsync(stream, cancellationToken);
        }

        return new UploadedDocument(fileName, $"/documents/{fileName}");
    }
}

public sealed record UploadedDocument(string FileName, string Url);