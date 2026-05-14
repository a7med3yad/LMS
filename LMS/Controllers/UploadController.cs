using LMS.Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LMS.Api.Controllers;

/// <summary>
/// Handles file uploads to Oracle Cloud Object Storage.
/// Returns public URLs that are then stored in the corresponding DB entity.
/// 
/// Flow:
///   1. Frontend POSTs file here → gets back { url }
///   2. Frontend sends the URL to the appropriate entity endpoint
///      (e.g., PATCH /api/v1/users/me/avatar with { avatarUrl: url })
/// </summary>
[ApiController]
[Route("api/v1/upload")]
[Authorize]
public class UploadController : ControllerBase
{
    private readonly IFileStorageService _storage;

    public UploadController(IFileStorageService storage) => _storage = storage;

    // ─── Avatar Upload ──────────────────────────────────────────
    /// <summary>
    /// Upload a user avatar image (max 5 MB, JPEG/PNG/WebP).
    /// Returns { url } to be saved via PATCH /api/v1/users/me/avatar.
    /// </summary>
    [HttpPost("avatar")]
    [RequestSizeLimit(5 * 1024 * 1024)] // 5 MB
    public async Task<IActionResult> UploadAvatar(IFormFile file)
    {
        ValidateImage(file, maxSizeMb: 5);

        var url = await _storage.UploadFileAsync(
            file.OpenReadStream(),
            file.FileName,
            file.ContentType,
            "avatars");

        return Ok(new { url });
    }

    // ─── Course Thumbnail Upload ────────────────────────────────
    /// <summary>
    /// Upload a course thumbnail image (max 10 MB, JPEG/PNG/WebP).
    /// Returns { url } to be saved via POST/PUT /api/v1/courses.
    /// </summary>
    [HttpPost("thumbnail")]
    [RequestSizeLimit(10 * 1024 * 1024)] // 10 MB
    public async Task<IActionResult> UploadThumbnail(IFormFile file)
    {
        ValidateImage(file, maxSizeMb: 10);

        var url = await _storage.UploadFileAsync(
            file.OpenReadStream(),
            file.FileName,
            file.ContentType,
            "thumbnails");

        return Ok(new { url });
    }

    // ─── Course Material Upload ─────────────────────────────────
    /// <summary>
    /// Upload course material (video, PDF, etc. — max 500 MB).
    /// Returns { url } to be saved via POST /api/v1/courses/{id}/materials.
    /// </summary>
    [HttpPost("material")]
    [Authorize(Roles = "Instructor")]
    [RequestSizeLimit(500L * 1024 * 1024)] // 500 MB
    [RequestFormLimits(MultipartBodyLengthLimit = 500L * 1024 * 1024)]
    public async Task<IActionResult> UploadMaterial(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest(new { message = "No file provided" });

        var url = await _storage.UploadFileAsync(
            file.OpenReadStream(),
            file.FileName,
            file.ContentType,
            "materials");

        return Ok(new { url });
    }

    // ─── Assignment Submission Upload ───────────────────────────
    /// <summary>
    /// Upload an assignment submission file (max 50 MB).
    /// Returns { url } to be saved via POST .../assignments/{id}/submit.
    /// </summary>
    [HttpPost("submission")]
    [Authorize(Roles = "Student")]
    [RequestSizeLimit(50 * 1024 * 1024)] // 50 MB
    public async Task<IActionResult> UploadSubmission(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest(new { message = "No file provided" });

        var url = await _storage.UploadFileAsync(
            file.OpenReadStream(),
            file.FileName,
            file.ContentType,
            "submissions");

        return Ok(new { url });
    }

    // ─── Generic Attachment Upload ──────────────────────────────
    /// <summary>
    /// Upload a generic attachment (max 25 MB).
    /// </summary>
    [HttpPost("attachment")]
    [RequestSizeLimit(25 * 1024 * 1024)] // 25 MB
    public async Task<IActionResult> UploadAttachment(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest(new { message = "No file provided" });

        var url = await _storage.UploadFileAsync(
            file.OpenReadStream(),
            file.FileName,
            file.ContentType,
            "attachments");

        return Ok(new { url });
    }

    // ─── Delete File ────────────────────────────────────────────
    /// <summary>
    /// Delete a file from storage by its public URL.
    /// </summary>
    [HttpDelete]
    [Authorize(Roles = "Instructor,Admin")]
    public async Task<IActionResult> DeleteFile([FromQuery] string fileUrl)
    {
        if (string.IsNullOrWhiteSpace(fileUrl))
            return BadRequest(new { message = "fileUrl is required" });

        await _storage.DeleteFileAsync(fileUrl);
        return NoContent();
    }

    // ─── Helpers ────────────────────────────────────────────────
    private static void ValidateImage(IFormFile? file, int maxSizeMb)
    {
        if (file == null || file.Length == 0)
            throw new BadHttpRequestException("No file provided");

        if (file.Length > maxSizeMb * 1024 * 1024)
            throw new BadHttpRequestException($"File size exceeds {maxSizeMb} MB limit");

        var allowedTypes = new[] { "image/jpeg", "image/png", "image/webp" };
        if (!allowedTypes.Contains(file.ContentType.ToLower()))
            throw new BadHttpRequestException("Only JPEG, PNG, and WebP images are allowed");
    }
}
