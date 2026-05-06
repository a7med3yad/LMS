using LMS.Application.DTOs.Exams;
using MediatR;

namespace LMS.Application.Command.Exam;

public record UpdateQuestionCommand(Guid QuestionId, Guid InstructorId, UpdateQuestionDto Dto)
    : IRequest<QuestionDto>;

public record DeleteQuestionCommand(Guid QuestionId, Guid InstructorId) : IRequest;
