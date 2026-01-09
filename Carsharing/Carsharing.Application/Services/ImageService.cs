using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Util;
using Carsharing.Application.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Carsharing.Application.Services;

public class ImageService : IImageService
{
    private readonly IAmazonS3 _s3Client;
    private readonly string _bucketName;
    private readonly string _serviceUrl;
    private readonly string[] _allowedExtensions = [".jpg", ".jpeg", ".png"];

    public ImageService(IAmazonS3 s3Client, IConfiguration configuration)
    {
        _s3Client = s3Client;
        _bucketName = configuration["Minio:BucketName"] ?? "default-bucket";
        _serviceUrl = configuration["Minio:ServiceURL"] ?? "http://localhost:9000";
    }

    public async Task<string> SaveCarImageAsync(IFormFile file)
    {
        return await SaveFileInternalAsync(file, "cars");
    }

    public async Task<string> SaveDocumentImageAsync(IFormFile file)
    {
        return await SaveFileInternalAsync(file, "documents");
    }

    private async Task<string> SaveFileInternalAsync(IFormFile file, string subFolder)
    {
        var ext = Path.GetExtension(file.FileName).ToLowerInvariant();

        if (!_allowedExtensions.Contains(ext))
            throw new ArgumentException($"Неподдерживаемый формат файла. Разрешены: {string.Join(", ", _allowedExtensions)}");

        // Проверяем, существует ли бакет, если нет — создаем
        await EnsureBucketExistsAsync();

        var fileName = $"{Guid.NewGuid()}{ext}";
        var objectKey = $"images/{subFolder}/{fileName}"; // Путь внутри бакета (ключ)

        var putRequest = new PutObjectRequest
        {
            BucketName = _bucketName,
            Key = objectKey,
            InputStream = file.OpenReadStream(),
            ContentType = file.ContentType
        };

        await _s3Client.PutObjectAsync(putRequest);

        // Возвращаем полный URL к файлу
        // Внимание: для MinIO URL обычно строится так: Host/BucketName/Key
        return $"{_serviceUrl}/{_bucketName}/{objectKey}";
    }

    public async void DeleteFile(string fileUrl)
    {
        if (string.IsNullOrEmpty(fileUrl)) return;

        try
        {
            // Нам нужно извлечь "Key" (путь внутри бакета) из полного URL.
            // URL: http://localhost:9000/carsharing-images/images/cars/guid.jpg
            // Key: images/cars/guid.jpg

            // Простая логика парсинга (можно улучшить через Uri класс)
            var key = fileUrl.Replace($"{_serviceUrl}/{_bucketName}/", "");

            var deleteRequest = new DeleteObjectRequest
            {
                BucketName = _bucketName,
                Key = key
            };

            await _s3Client.DeleteObjectAsync(deleteRequest);
        }
        catch
        {
            // Логирование ошибки удаления
        }
    }

    private async Task EnsureBucketExistsAsync()
    {
        var bucketExists = await AmazonS3Util.DoesS3BucketExistV2Async(_s3Client, _bucketName);
        if (!bucketExists)
        {
            var putBucketRequest = new PutBucketRequest
            {
                BucketName = _bucketName,
                UseClientRegion = true
            };
            await _s3Client.PutBucketAsync(putBucketRequest);

            // Делаем бакет публичным для чтения (чтобы картинки открывались на фронте)
            // В продакшене политика может быть строже
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

            await _s3Client.PutBucketPolicyAsync(new PutBucketPolicyRequest
            {
                BucketName = _bucketName,
                Policy = policy
            });
        }
    }
}