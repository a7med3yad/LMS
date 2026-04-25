using LMS.Application.Abstraction.UnitOfWork;
using LMS.Application.Common.Exceptions;
using LMS.Application.DTOs.Enrollments;
using LMS.Application.DTOs.Users;
using LMS.Domain.DTOs.Enrollments;
using LMS.Domain.Models.Enums;
using MediatR;

namespace LMS.Application.Command.Enrollment;

public class EnrollHandler : IRequestHandler<EnrollCommand, EnrollmentDto>
{
    private readonly IUnitOfWork _uow;
    public EnrollHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<EnrollmentDto> Handle(EnrollCommand request, CancellationToken ct)
    {
        if (await _uow.Enrollments.IsEnrolledAsync(request.StudentId, request.Dto.CourseId, ct))
            throw new ConflictException("Already enrolled in this course.");

        await _uow.Courses.GetByIdAsync(request.Dto.CourseId, ct)
            is not { } course
            ? throw new NotFoundException("Course", request.Dto.CourseId)
            : course;

        var enrollment = new Domain.Models.Enrollment
        {
            Id = Guid.NewGuid(),
            StudentId = request.StudentId,
            CourseId = request.Dto.CourseId,
            Status = EnrollmentStatus.Active
        };

        await _uow.Enrollments.AddAsync(enrollment, ct);
        await _uow.SaveChangesAsync(ct);

        var full = await _uow.Enrollments.GetByStudentAndCourseAsync(
            request.StudentId, request.Dto.CourseId, ct)!;

        return new EnrollmentDto(full!.Id, full.CourseId,
            full.Course.TitleAr, full.Course.TitleEn,
            new UserSummaryDto(full.Student.Id.ToString(), full.Student.FullName,
                               full.Student.Email!, full.Student.AvatarUrl,
                               full.Student.Role.ToString()),
            full.Status, full.PaidAmount, full.Voucher?.Code,
            full.EnrolledAt, full.CompletedAt);
    }
}
