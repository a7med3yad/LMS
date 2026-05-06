using LMS.Application.Abstraction.UnitOfWork;
using LMS.Application.Common.Exceptions;
using LMS.Application.DTOs.Vouchers;
using MediatR;

namespace LMS.Application.Command.Voucher;

public class CreateVoucherHandler : IRequestHandler<CreateVoucherCommand, VoucherDto>
{
    private readonly IUnitOfWork _uow;
    public CreateVoucherHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<VoucherDto> Handle(CreateVoucherCommand req, CancellationToken ct)
    {
        if (!await _uow.Courses.IsInstructorOfCourseAsync(req.InstructorId, req.Dto.CourseId, ct))
            throw new ForbiddenException();

        if (await _uow.Vouchers.AnyAsync(v => v.Code == req.Dto.Code, ct))
            throw new ConflictException("Voucher code already exists.");

        var course = await _uow.Courses.GetByIdAsync(req.Dto.CourseId, ct)!;

        var voucher = new Domain.Models.Voucher
        {
            Id = Guid.NewGuid(),
            Code = req.Dto.Code,
            CourseId = req.Dto.CourseId,
            DiscountPercent = req.Dto.DiscountPercent,
            DiscountAmount = req.Dto.DiscountAmount,
            MaxUses = req.Dto.MaxUses,
            ExpiresAt = req.Dto.ExpiresAt
        };

        await _uow.Vouchers.AddAsync(voucher, ct);
        await _uow.SaveChangesAsync(ct);

        return new VoucherDto(voucher.Id, voucher.Code, voucher.CourseId,
            course!.TitleEn, voucher.DiscountPercent, voucher.DiscountAmount,
            voucher.MaxUses, voucher.UsedCount, voucher.ExpiresAt,
            voucher.IsActive, voucher.CreatedAt);
    }
}