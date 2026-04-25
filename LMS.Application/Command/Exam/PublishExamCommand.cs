using MediatR;

namespace LMS.Application.Command.Exam;

public record PublishExamCommand(Guid ExamId, Guid InstructorId) : IRequest;
