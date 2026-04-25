using MediatR;

namespace LMS.Application.Command.Course;

public record DeleteCourseCommand(Guid CourseId, Guid RequesterId) : IRequest;
