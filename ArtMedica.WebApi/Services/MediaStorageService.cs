
using System.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Webp;

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

        var outputExtension = isImage
            ? ".webp"
            : ".mp4";

        var fileName = $"{Guid.NewGuid():N}{outputExtension}";
        var path = Path.Combine(directory, fileName);

        try
        {
            if (isImage)
            {
                await ConvertImageAsync(
                    file, path, cancellationToken);
            }
            else
            {
                await ConvertVideoAsync(
                    file, path, directory, cancellationToken);
            }

            return new UploadedMedia(
                fileName,
                type,
                $"/uploads/media/{fileName}");
        }
        catch
        {
            if (File.Exists(path))
                File.Delete(path);

            throw;
        }
    }

    private static async Task ConvertImageAsync(
        IFormFile file,
        string outputPath,
        CancellationToken cancellationToken)
    {
        await using var input = file.OpenReadStream();

        using var image = await Image.LoadAsync(
            input, cancellationToken);

        var encoder = new WebpEncoder
        {
            Quality = 85,
            FileFormat = WebpFileFormatType.Lossy
        };

        await image.SaveAsync(
            outputPath,
            encoder,
            cancellationToken);
    }

    private static async Task ConvertVideoAsync(
        IFormFile file,
        string outputPath,
        string directory,
        CancellationToken cancellationToken)
    {
        var inputExtension = Path.GetExtension(file.FileName);

        var tempInput = Path.Combine(
            directory,
            $"{Guid.NewGuid():N}{inputExtension}");

        var tempOutput = Path.Combine(
            directory,
            $"{Guid.NewGuid():N}.mp4");

        try
        {
            await using (var input = new FileStream(
                tempInput,
                FileMode.CreateNew,
                FileAccess.Write,
                FileShare.None,
                81920,
                useAsync: true))
            {
                await file.CopyToAsync(input, cancellationToken);
            }

            var startInfo = new ProcessStartInfo
            {
                FileName = FfmpegPath,
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardError = true
            };

            startInfo.ArgumentList.Add("-nostdin");
            startInfo.ArgumentList.Add("-hide_banner");
            startInfo.ArgumentList.Add("-loglevel");
            startInfo.ArgumentList.Add("error");

            startInfo.ArgumentList.Add("-i");
            startInfo.ArgumentList.Add(tempInput);

            startInfo.ArgumentList.Add("-map");
            startInfo.ArgumentList.Add("0:v:0");

            startInfo.ArgumentList.Add("-map");
            startInfo.ArgumentList.Add("0:a:0?");

            startInfo.ArgumentList.Add("-c:v");
            startInfo.ArgumentList.Add("libx264");

            startInfo.ArgumentList.Add("-preset");
            startInfo.ArgumentList.Add("medium");

            startInfo.ArgumentList.Add("-crf");
            startInfo.ArgumentList.Add("23");

            startInfo.ArgumentList.Add("-pix_fmt");
            startInfo.ArgumentList.Add("yuv420p");

            startInfo.ArgumentList.Add("-vf");
            startInfo.ArgumentList.Add(
                "scale=trunc(iw/2)*2:trunc(ih/2)*2");

            startInfo.ArgumentList.Add("-c:a");
            startInfo.ArgumentList.Add("aac");

            startInfo.ArgumentList.Add("-b:a");
            startInfo.ArgumentList.Add("128k");

            startInfo.ArgumentList.Add("-movflags");
            startInfo.ArgumentList.Add("+faststart");

            startInfo.ArgumentList.Add("-y");
            startInfo.ArgumentList.Add(tempOutput);

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
                    $"Ошибка конвертации видео: {error}");

            if (!File.Exists(tempOutput) ||
                new FileInfo(tempOutput).Length == 0)
            {
                throw new InvalidOperationException(
                    "FFmpeg не создал выходной файл.");
            }

            File.Move(tempOutput, outputPath);
        }
        finally
        {
            if (File.Exists(tempInput))
                File.Delete(tempInput);

            if (File.Exists(tempOutput))
                File.Delete(tempOutput);
        }
    }
}

public sealed record UploadedMedia(
    string FileName,
    string Type,
    string Url);
