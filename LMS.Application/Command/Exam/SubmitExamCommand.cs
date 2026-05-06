using LMS.Application.DTOs.Exams;
using MediatR;

namespace LMS.Application.Command.Exam;

public record SubmitExamCommand(SubmitExamDto Dto, Guid StudentId) : IRequest<ExamAttemptDto>;
