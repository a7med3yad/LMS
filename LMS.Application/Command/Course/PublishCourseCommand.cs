using MediatR;

namespace LMS.Application.Command.Course;

public record PublishCourseCommand(Guid CourseId, Guid RequesterId) : IRequest;
