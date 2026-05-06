using MediatR;

namespace LMS.Application.Command.Exam;

public record DeleteExamCommand(Guid ExamId, Guid InstructorId) : IRequest;
