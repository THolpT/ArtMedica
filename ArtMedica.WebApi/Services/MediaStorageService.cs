
using System.Diagnostics;
using Microsoft.AspNetCore.Hosting;

namespace ArtMedica.WebApi.Services;

public sealed class MediaStorageService
{
    private readonly IWebHostEnvironment _environment;

    private static readonly HashSet<string> ImageExtensions =
        new(StringComparer.OrdinalIgnoreCase)
        {
            ".jpg", ".jpeg", ".png", ".gif",
            ".webp", ".avif"
        };

    private static readonly HashSet<string> VideoExtensions =
        new(StringComparer.OrdinalIgnoreCase)
        {
            ".mp4", ".webm", ".mov",
            ".avi", ".mkv", ".m4v"
        };

    private const long MaxFileSize = 100 * 1024 * 1024;

    private const string FfmpegPath = "ffmpeg";

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

        var isImage = ImageExtensions.Contains(extension);
        var isVideo = VideoExtensions.Contains(extension);

        if (!isImage && !isVideo)
            throw new InvalidOperationException(
                $"Формат {extension} не поддерживается.");

        var webRoot = _environment.WebRootPath
            ?? Path.Combine(_environment.ContentRootPath, "wwwroot");

        var directory = Path.Combine(
            webRoot, "uploads", "media");

        Directory.CreateDirectory(directory);

        var type = isImage ? "image" : "video";

        var outputExtension = isImage ? ".webp" : ".mp4";

        var fileName = $"{Guid.NewGuid():N}{outputExtension}";
        var outputPath = Path.Combine(directory, fileName);

        try
        {
            if (isImage)
            {
                await ConvertImageAsync(
                    file, outputPath, directory, cancellationToken);
            }
            else
            {
                await ConvertVideoAsync(
                    file, outputPath, directory, cancellationToken);
            }

            return new UploadedMedia(
                fileName,
                type,
                $"/uploads/media/{fileName}");
        }
        catch
        {
            if (File.Exists(outputPath))
                File.Delete(outputPath);

            throw;
        }
    }

    // ==========================================
    // IMAGE → WEBP
    // ==========================================

    private static async Task ConvertImageAsync(
        IFormFile file,
        string outputPath,
        string directory,
        CancellationToken cancellationToken)
    {
        var tempInput = Path.Combine(
            directory, $"{Guid.NewGuid():N}.tmp");

        try
        {
            await SaveTempFileAsync(
                file, tempInput, cancellationToken);

            var arguments = new[]
            {
                "-nostdin",
                "-hide_banner",
                "-loglevel", "error",

                "-i", tempInput,

                "-frames:v", "1",

                "-c:v", "libwebp",
                "-quality", "85",
                "-compression_level", "4",

                "-f", "webp",
                "-y", outputPath
            };

            await RunFfmpegAsync(arguments, cancellationToken);

            if (!File.Exists(outputPath) ||
                new FileInfo(outputPath).Length == 0)
            {
                throw new InvalidOperationException(
                    "FFmpeg не создал изображение WebP.");
            }
        }
        finally
        {
            if (File.Exists(tempInput))
                File.Delete(tempInput);
        }
    }

    // ==========================================
    // VIDEO → MP4
    // ==========================================

    private static async Task ConvertVideoAsync(
        IFormFile file,
        string outputPath,
        string directory,
        CancellationToken cancellationToken)
    {
        var tempInput = Path.Combine(
            directory, $"{Guid.NewGuid():N}.tmp");

        try
        {
            await SaveTempFileAsync(
                file, tempInput, cancellationToken);

            var arguments = new[]
            {
                "-nostdin",
                "-hide_banner",
                "-loglevel", "error",

                "-i", tempInput,

                "-map", "0:v:0",
                "-map", "0:a:0?",

                "-c:v", "libx264",
                "-preset", "medium",
                "-crf", "23",
                "-pix_fmt", "yuv420p",

                "-vf", "scale=trunc(iw/2)*2:trunc(ih/2)*2",

                "-c:a", "aac",
                "-b:a", "128k",

                "-movflags", "+faststart",

                "-f", "mp4",
                "-y", outputPath
            };

            await RunFfmpegAsync(arguments, cancellationToken);

            if (!File.Exists(outputPath) ||
                new FileInfo(outputPath).Length == 0)
            {
                throw new InvalidOperationException(
                    "FFmpeg не создал выходное видео.");
            }
        }
        finally
        {
            if (File.Exists(tempInput))
                File.Delete(tempInput);
        }
    }

    // ==========================================
    // SAVE TEMP FILE
    // ==========================================

    private static async Task SaveTempFileAsync(
        IFormFile file,
        string path,
        CancellationToken cancellationToken)
    {
        await using var output = new FileStream(
            path,
            FileMode.CreateNew,
            FileAccess.Write,
            FileShare.None,
            81920,
            useAsync: true);

        await file.CopyToAsync(output, cancellationToken);
    }

    // ==========================================
    // RUN FFMPEG
    // ==========================================

    private static async Task RunFfmpegAsync(
        IEnumerable<string> arguments,
        CancellationToken cancellationToken)
    {
        var startInfo = new ProcessStartInfo
        {
            FileName = FfmpegPath,
            UseShellExecute = false,
            CreateNoWindow = true,
            RedirectStandardError = true
        };

        foreach (var argument in arguments)
            startInfo.ArgumentList.Add(argument);

        using var process = new Process
        {
            StartInfo = startInfo
        };

        if (!process.Start())
            throw new InvalidOperationException(
                "Не удалось запустить FFmpeg.");

        var errorTask = process.StandardError.ReadToEndAsync();

        try
        {
            await process.WaitForExitAsync(cancellationToken);
        }
        catch
        {
            if (!process.HasExited)
                process.Kill(entireProcessTree: true);

            await process.WaitForExitAsync();

            throw;
        }

        var error = await errorTask;

        if (process.ExitCode != 0)
            throw new InvalidOperationException(
                $"Ошибка FFmpeg: {error}");
    }
}

public sealed record UploadedMedia(
    string FileName,
    string Type,
    string Url);
