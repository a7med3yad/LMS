namespace LMS.Models
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public string Token { get; set; }
        public DateTime ExpiresAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsRevoked { get; set; } = false;

        public Guid UserId { get; set; }
        public ApplicationUser User { get; set; }

        public string? ReplacedByToken { get; set; }

        public bool IsExpired => DateTime.UtcNow > ExpiresAt;
        public bool IsActive => !IsRevoked && !IsExpired;
    }
}
