namespace LMS.Domain.Common;

/// <summary>
/// Configuration POCO for Oracle Cloud Infrastructure (OCI) Object Storage.
/// Binds to the "OracleObjectStorage" section in appsettings.json.
/// Uses the S3-compatible API so we can leverage the AWS SDK.
/// </summary>
public class OciSettings
{
    public string Region { get; set; } = string.Empty;
    public string Namespace { get; set; } = string.Empty;
    public string BucketName { get; set; } = string.Empty;
    public string AccessKeyId { get; set; } = string.Empty;
    public string SecretAccessKey { get; set; } = string.Empty;

    /// <summary>
    /// S3-compatible endpoint: https://{namespace}.compat.objectstorage.{region}.oraclecloud.com
    /// </summary>
    public string S3Endpoint { get; set; } = string.Empty;

    /// <summary>
    /// Public base URL for stored objects:
    /// https://objectstorage.{region}.oraclecloud.com/n/{namespace}/b/{bucket}/o
    /// </summary>
    public string PublicBaseUrl { get; set; } = string.Empty;
}
