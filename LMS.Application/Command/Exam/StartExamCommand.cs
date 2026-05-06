using LMS.Application.DTOs.Exams;
using MediatR;

namespace LMS.Application.Command.Exam;

public record StartExamCommand(Guid ExamId, Guid StudentId) : IRequest<StartExamResponseDto>;
