using LMS.Models.Enums;

namespace LMS.Entities
{

    public class Material
    {
        public Guid Id { get; set; }
        public string TitleAr { get; set; } = string.Empty;
        public string TitleEn { get; set; } = string.Empty;
        public string? DescriptionAr { get; set; }
        public string? DescriptionEn { get; set; } 
        public MaterialType Type { get; set; }
        public string ContentUrl { get; set; } = string.Empty;
        public string? TextContent { get; set; } 
        public int Order { get; set; }
        public bool IsPublished { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public Guid CourseId { get; set; }
        public Course Course { get; set; } = null!;
    }
}
