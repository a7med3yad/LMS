using LMS.Application.DTOs.Enrollments;
using MediatR;

namespace LMS.Application.Command.Enrollment;

public record EnrollCommand(Guid StudentId, EnrollRequestDto Dto) : IRequest<EnrollmentDto>;
