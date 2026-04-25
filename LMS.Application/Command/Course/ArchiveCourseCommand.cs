using MediatR;

namespace LMS.Application.Command.Course;

public record ArchiveCourseCommand(Guid CourseId, Guid RequesterId) : IRequest;
