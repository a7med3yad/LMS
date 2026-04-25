using LMS.Application.DTOs.Courses;
using MediatR;

namespace LMS.Application.Command.Course;

public record UpdateCourseCommand(Guid CourseId, Guid RequesterId, UpdateCourseDto Dto)
    : IRequest<CourseDto>;
