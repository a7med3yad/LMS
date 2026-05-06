<<<<<<< HEAD
using LMS.Application.DTOs.Enrollments;
=======
﻿using LMS.Application.DTOs.Enrollments;
using LMS.Domain.DTOs.Enrollments;
using LMS.Domain.Models;
>>>>>>> 4450aad95aa0059499e5c99c961c831b227af253
using MediatR;

namespace LMS.Application.Command.Enrollment;

public record EnrollCommand(Guid StudentId, EnrollRequestDto Dto) : IRequest<EnrollmentDto>;
