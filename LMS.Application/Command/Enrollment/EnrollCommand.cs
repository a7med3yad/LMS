using LMS.Application.DTOs.Enrollments;
using LMS.Domain.DTOs.Enrollments;
using LMS.Domain.Models;
using MediatR;

namespace LMS.Application.Command.Enrollment;

public record EnrollCommand(Guid StudentId, EnrollRequestDto Dto) : IRequest<EnrollmentDto>;
