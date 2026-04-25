using LMS.Application.Abstraction.UnitOfWork;
using LMS.Application.Common.Exceptions;
using LMS.Application.DTOs.Courses;
using LMS.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace LMS.Application.Command.Course;

public class CreateCourseHandler : IRequestHandler<CreateCourseCommand, CourseDto>
{
    private readonly IUnitOfWork _uow;
    private readonly UserManager<ApplicationUser> _userManager;

    public CreateCourseHandler(IUnitOfWork uow, UserManager<ApplicationUser> um)
    {
        _uow = uow;
        _userManager = um;
    }

    public async Task<CourseDto> Handle(CreateCourseCommand request, CancellationToken ct)
    {
        var instructor = await _userManager.FindByIdAsync(request.InstructorId.ToString())
            ?? throw new NotFoundException("Instructor", request.InstructorId);

        var course = new Domain.Models.Course
        {
            Id = Guid.NewGuid(),
            TitleAr = request.Dto.TitleAr,
            TitleEn = request.Dto.TitleEn,
            DescriptionAr = request.Dto.DescriptionAr,
            DescriptionEn = request.Dto.DescriptionEn,
            ThumbnailUrl = request.Dto.ThumbnailUrl,
            Price = request.Dto.Price,
            InstructorId = request.InstructorId,
            Instructor = instructor
        };

        await _uow.Courses.AddAsync(course, ct);
        await _uow.SaveChangesAsync(ct);

        return new CourseDto(
            course.Id, course.TitleAr, course.TitleEn,
            course.DescriptionAr, course.DescriptionEn,
            course.ThumbnailUrl, course.Price, course.Status,
            new LMS.Domain.DTOs.Users.UserSummaryDto(
                instructor.Id.ToString(),
                instructor.FullName,
                instructor.Email!,
                instructor.AvatarUrl,
                instructor.Role.ToString()
            ),
            0, 0, course.CreatedAt, course.UpdatedAt);
    }
  
}