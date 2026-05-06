using LMS.Application.DTOs.Exams;
using MediatR;

namespace LMS.Application.Command.Exam;

public record AddQuestionCommand(Guid ExamId, Guid InstructorId, CreateQuestionDto Dto) : IRequest<QuestionDto>;
