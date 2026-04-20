using LMS.Common.Pagination;
using LMS.Models.Enums;

namespace LMS.DTOs.Courses;

public class CourseFilterDto : PagedRequest
{
    public CourseStatus? Status { get; set; }
    public Guid? InstructorId { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
}
