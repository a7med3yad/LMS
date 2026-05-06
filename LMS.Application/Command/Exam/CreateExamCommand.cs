using LMS.Application.DTOs.Exams;
using LMS.Domain.Models;
using MediatR;

namespace LMS.Application.Command.Exam;

public record CreateExamCommand(Guid CourseId, Guid InstructorId, CreateExamDto Dto)
    : IRequest<ExamDto>;
