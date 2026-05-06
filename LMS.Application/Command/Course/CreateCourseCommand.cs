using LMS.Application.DTOs.Courses;
using LMS.Domain.Models.Enums;
using MediatR;

namespace LMS.Application.Command.Course;

public record CreateCourseCommand(Guid InstructorId, CreateCourseDto Dto) : IRequest<CourseDto>;
