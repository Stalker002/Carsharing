using System.Text.Json;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Util;
using Carsharing.Application.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Carsharing.Application.Services;

public class ImageService(
    IAmazonS3 s3Client,
    IOptions<MinioStorageOptions> minioOptions,
    IOptions<FileUploadOptions> fileUploadOptions) : IImageService
{
    private static readonly HashSet<string> AllowedExtensions = [".jpg", ".jpeg", ".png"];

    private static readonly Dictionary<string, string[]> AllowedMimeTypes = new(StringComparer.OrdinalIgnoreCase)
    {
        [".jpg"] = ["image/jpeg"],
        [".jpeg"] = ["image/jpeg"],
        [".png"] = ["image/png"]
    };

    private readonly FileUploadOptions _fileUploadOptions = fileUploadOptions.Value;
    private readonly MinioStorageOptions _minioOptions = minioOptions.Value;

    public async Task<string> SaveCarImageAsync(IFormFile file, CancellationToken cancellationToken)
    {
        return await SaveFileInternalAsync(
            file,
            _minioOptions.BucketName,
            _minioOptions.PublicURL,
            "cars",
            _fileUploadOptions.MaxCarImageBytes,
            cancellationToken);
    }

    public async Task<string> SaveDocumentImageAsync(IFormFile file, CancellationToken cancellationToken)
    {
        return await SaveFileInternalAsync(
            file,
            _minioOptions.BucketName,
            _minioOptions.PublicURL,
            "documents",
            _fileUploadOptions.MaxDocumentImageBytes,
            cancellationToken);
    }

    public async Task DeleteFile(string fileUrl, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(fileUrl))
            return;

        try
        {
            var (bucketName, key) = ParseStoredFileReference(fileUrl);

            await s3Client.DeleteObjectAsync(new DeleteObjectRequest
            {
                BucketName = bucketName,
                Key = key
            }, cancellationToken);
        }
        catch
        {
            // Ignore cleanup failures to avoid masking the original business error.
        }
    }

    private async Task<string> SaveFileInternalAsync(
        IFormFile file,
        string bucketName,
        string baseUrl,
        string subFolder,
        long maxBytes,
        CancellationToken cancellationToken)
    {
        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

        if (!AllowedExtensions.Contains(extension))
            throw new ArgumentException(
                $"Неподдерживаемый формат файла. Разрешены: {string.Join(", ", AllowedExtensions)}");

        if (file.Length <= 0)
            throw new ArgumentException("Файл пустой.");

        if (file.Length > maxBytes)
            throw new ArgumentException($"Размер файла превышает допустимый лимит {maxBytes / (1024 * 1024)} MB.");

        ValidateMimeType(file.ContentType, extension);

        await using var buffer = new MemoryStream();
        await file.CopyToAsync(buffer, cancellationToken);
        await EnsureBucketExistsAsync(bucketName, cancellationToken);

        var fileName = $"{Guid.NewGuid()}{extension}";
        var objectKey = $"images/{subFolder}/{fileName}";
        buffer.Position = 0;

        var putRequest = new PutObjectRequest
        {
            BucketName = bucketName,
            Key = objectKey,
            InputStream = buffer,
            ContentType = file.ContentType
        };

        await s3Client.PutObjectAsync(putRequest, cancellationToken);

        return $"{baseUrl.TrimEnd('/')}/{bucketName}/{objectKey}";
    }

    private async Task EnsureBucketExistsAsync(string bucketName, CancellationToken cancellationToken)
    {
        var bucketExists = await AmazonS3Util.DoesS3BucketExistV2Async(s3Client, bucketName);
        if (!bucketExists)
            await s3Client.PutBucketAsync(new PutBucketRequest
            {
                BucketName = bucketName,
                UseClientRegion = true
            }, cancellationToken);

        await ApplyPublicReadPolicyAsync(bucketName, cancellationToken);
    }

    private async Task ApplyPublicReadPolicyAsync(string bucketName, CancellationToken cancellationToken)
    {
        var policyDocument = new
        {
            Version = "2012-10-17",
            Statement = new[]
            {
                new
                {
                    Effect = "Allow",
                    Principal = new { AWS = new[] { "*" } },
                    Action = new[] { "s3:GetObject" },
                    Resource = new[] { $"arn:aws:s3:::{bucketName}/images/cars/*" }
                }
            }
        };

        await s3Client.PutBucketPolicyAsync(new PutBucketPolicyRequest
        {
            BucketName = bucketName,
            Policy = JsonSerializer.Serialize(policyDocument)
        }, cancellationToken);
    }

    private static void ValidateMimeType(string? contentType, string extension)
    {
        if (string.IsNullOrWhiteSpace(contentType))
            throw new ArgumentException("Не удалось определить MIME-тип файла.");

        if (!AllowedMimeTypes.TryGetValue(extension, out var allowedMimeTypes) ||
            !allowedMimeTypes.Contains(contentType, StringComparer.OrdinalIgnoreCase))
            throw new ArgumentException("Неподдерживаемый MIME-тип файла.");
    }

    private static (string BucketName, string Key) ParseStoredFileReference(string fileUrl)
    {
        if (fileUrl.StartsWith("s3://", StringComparison.OrdinalIgnoreCase))
        {
            var reference = fileUrl["s3://".Length..];
            var separatorIndex = reference.IndexOf('/');
            if (separatorIndex <= 0 || separatorIndex >= reference.Length - 1)
                throw new ArgumentException("Некорректная ссылка на файл.");

            return (reference[..separatorIndex], reference[(separatorIndex + 1)..]);
        }

        var uri = new Uri(fileUrl, UriKind.Absolute);
        var segments = uri.AbsolutePath.Trim('/').Split('/', 2, StringSplitOptions.RemoveEmptyEntries);
        if (segments.Length != 2)
            throw new ArgumentException("Некорректная ссылка на файл.");

        return (segments[0], segments[1]);
    }
}