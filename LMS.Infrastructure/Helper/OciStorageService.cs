using Amazon.S3;
using Amazon.S3.Model;
using LMS.Domain.Common;
using LMS.Domain.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace LMS.Infrastructure.Helper;

/// <summary>
/// Oracle Cloud Infrastructure Object Storage service using the S3-compatible API.
/// Uploads files and returns public URLs that are stored in the database.
/// </summary>
public class OciStorageService : IFileStorageService
{
    private readonly IAmazonS3 _s3Client;
    private readonly OciSettings _settings;
    private readonly ILogger<OciStorageService> _logger;

    public OciStorageService(
        IAmazonS3 s3Client,
        IOptions<OciSettings> settings,
        ILogger<OciStorageService> logger)
    {
        _s3Client = s3Client;
        _settings = settings.Value;
        _logger = logger;
    }

    public async Task<string> UploadFileAsync(
        Stream fileStream,
        string fileName,
        string contentType,
        string folder)
    {
        var ext = Path.GetExtension(fileName).ToLowerInvariant();
        var key = $"{folder}/{Guid.NewGuid()}{ext}";

        var request = new PutObjectRequest
        {
            BucketName = _settings.BucketName,
            Key = key,
            InputStream = fileStream,
            ContentType = contentType,
            CannedACL = S3CannedACL.PublicRead,
        };

        await _s3Client.PutObjectAsync(request);

        var publicUrl = $"{_settings.PublicBaseUrl.TrimEnd('/')}/{key}";

        _logger.LogInformation("Uploaded file to OCI: {Key} → {Url}", key, publicUrl);

        return publicUrl;
    }

    public async Task DeleteFileAsync(string fileUrl)
    {
        if (string.IsNullOrWhiteSpace(fileUrl))
            return;

        // Extract the object key from the full public URL
        var baseUrl = _settings.PublicBaseUrl.TrimEnd('/') + "/";
        var key = fileUrl.Replace(baseUrl, string.Empty);

        if (string.IsNullOrWhiteSpace(key))
        {
            _logger.LogWarning("Could not extract key from URL: {Url}", fileUrl);
            return;
        }

        var request = new DeleteObjectRequest
        {
            BucketName = _settings.BucketName,
            Key = key,
        };

        await _s3Client.DeleteObjectAsync(request);

        _logger.LogInformation("Deleted file from OCI: {Key}", key);
    }
}
