using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Util;
using Carsharing.Application.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Carsharing.Application.Services;

public class ImageService(IAmazonS3 s3Client, IConfiguration configuration) : IImageService
{
    private readonly string _bucketName = configuration["Minio:BucketName"] ?? "default-bucket";
    private readonly string _serviceUrl = configuration["Minio:ServiceURL"] ?? "http://localhost:9000";
    private readonly string[] _allowedExtensions = [".jpg", ".jpeg", ".png"];

    public async Task<string> SaveCarImageAsync(IFormFile file, CancellationToken cancellationToken)
    {
        return await SaveFileInternalAsync(file, "cars", cancellationToken);
    }

    public async Task<string> SaveDocumentImageAsync(IFormFile file, CancellationToken cancellationToken)
    {
        return await SaveFileInternalAsync(file, "documents", cancellationToken);
    }

    private async Task<string> SaveFileInternalAsync(IFormFile file, string subFolder, CancellationToken cancellationToken)
    {
        var ext = Path.GetExtension(file.FileName).ToLowerInvariant();

        if (!_allowedExtensions.Contains(ext))
            throw new ArgumentException($"Неподдерживаемый формат файла. Разрешены: {string.Join(", ", _allowedExtensions)}");

        await EnsureBucketExistsAsync(cancellationToken);

        var fileName = $"{Guid.NewGuid()}{ext}";
        var objectKey = $"images/{subFolder}/{fileName}";

        var putRequest = new PutObjectRequest
        {
            BucketName = _bucketName,
            Key = objectKey,
            InputStream = file.OpenReadStream(),
            ContentType = file.ContentType
        };

        await s3Client.PutObjectAsync(putRequest, cancellationToken);

        return $"{_serviceUrl}/{_bucketName}/{objectKey}";
    }

    public async Task DeleteFile(string fileUrl, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(fileUrl)) return;

        try
        {
            var key = fileUrl.Replace($"{_serviceUrl}/{_bucketName}/", "");

            var deleteRequest = new DeleteObjectRequest
            {
                BucketName = _bucketName,
                Key = key
            };

            await s3Client.DeleteObjectAsync(deleteRequest, cancellationToken);
        }
        catch
        {
            // Ignore cleanup failures to avoid masking the original business error.
        }
    }

    private async Task EnsureBucketExistsAsync(CancellationToken cancellationToken)
    {
        var bucketExists = await AmazonS3Util.DoesS3BucketExistV2Async(s3Client, _bucketName);
        if (!bucketExists)
        {
            var putBucketRequest = new PutBucketRequest
            {
                BucketName = _bucketName,
                UseClientRegion = true
            };
            await s3Client.PutBucketAsync(putBucketRequest, cancellationToken);

            string policy = $@"{{
                ""Version"": ""2012-10-17"",
                ""Statement"": [
                    {{
                        ""Effect"": ""Allow"",
                        ""Principal"": {{ ""AWS"": [""*""] }},
                        ""Action"": [""s3:GetObject""],
                        ""Resource"": [""arn:aws:s3:::{_bucketName}/*""]
                    }}
                ]
            }}";

            await s3Client.PutBucketPolicyAsync(new PutBucketPolicyRequest
            {
                BucketName = _bucketName,
                Policy = policy
            }, cancellationToken);
        }
    }
}
