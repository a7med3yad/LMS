using LMS.Application.DTOs.Exams;
using MediatR;

namespace LMS.Application.Command.Exam;

public record UpdateExamCommand(Guid ExamId, Guid InstructorId, UpdateExamDto Dto) : IRequest<ExamDto>;
