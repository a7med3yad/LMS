using LMS.Application.DTOs.Exams;
using MediatR;

namespace LMS.Application.Command.Exam;

public record GradeOpenEndedCommand(Guid ExamId, Guid InstructorId, GradeOpenEndedDto Dto) : IRequest;
