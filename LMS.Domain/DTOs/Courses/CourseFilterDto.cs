using LMS.Domain.Common.Pagination;
using LMS.Domain.Models.Enums;

namespace LMS.Domain.DTOs.Courses;

public class CourseFilterDto : PagedRequest
{
    public CourseStatus? Status { get; set; }
    public Guid? InstructorId { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
}
