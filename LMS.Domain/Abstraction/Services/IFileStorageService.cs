namespace LMS.Domain.Services;

/// <summary>
/// Abstraction for cloud file storage operations.
/// Returns public URLs that are stored directly in the database.
/// </summary>
public interface IFileStorageService
{
    /// <summary>
    /// Uploads a file to the specified folder and returns its public URL.
    /// </summary>
    /// <param name="fileStream">The file content stream.</param>
    /// <param name="fileName">Original file name (used for extension).</param>
    /// <param name="contentType">MIME type of the file.</param>
    /// <param name="folder">Target folder/prefix (e.g. "avatars", "thumbnails").</param>
    /// <returns>The public URL of the uploaded file.</returns>
    Task<string> UploadFileAsync(Stream fileStream, string fileName, string contentType, string folder);

    /// <summary>
    /// Deletes a file by its public URL.
    /// </summary>
    /// <param name="fileUrl">The full public URL of the file to delete.</param>
    Task DeleteFileAsync(string fileUrl);
}
