using LMS.Application.Abstraction.UnitOfWork;
using LMS.Application.Common.Exceptions;
using LMS.Application.DTOs.Vouchers;
using MediatR;

namespace LMS.Application.Command.Voucher;

public class UpdateVoucherHandler : IRequestHandler<UpdateVoucherCommand, VoucherDto>
{
    private readonly IUnitOfWork _uow;
    public UpdateVoucherHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<VoucherDto> Handle(UpdateVoucherCommand req, CancellationToken ct)
    {
        var voucher = await _uow.Vouchers.GetByIdAsync(req.VoucherId, ct)
            ?? throw new NotFoundException("Voucher", req.VoucherId);

        if (!await _uow.Courses.IsInstructorOfCourseAsync(req.InstructorId, voucher.CourseId, ct))
            throw new ForbiddenException();

        if (req.Dto.DiscountPercent.HasValue) voucher.DiscountPercent = req.Dto.DiscountPercent.Value;
        if (req.Dto.DiscountAmount.HasValue) voucher.DiscountAmount = req.Dto.DiscountAmount.Value;
        if (req.Dto.MaxUses.HasValue) voucher.MaxUses = req.Dto.MaxUses.Value;
        if (req.Dto.ExpiresAt.HasValue) voucher.ExpiresAt = req.Dto.ExpiresAt.Value;
        if (req.Dto.IsActive.HasValue) voucher.IsActive = req.Dto.IsActive.Value;

        _uow.Vouchers.Update(voucher);
        await _uow.SaveChangesAsync(ct);

        var course = await _uow.Courses.GetByIdAsync(voucher.CourseId, ct);
        return new VoucherDto(voucher.Id, voucher.Code, voucher.CourseId,
            course!.TitleEn, voucher.DiscountPercent, voucher.DiscountAmount,
            voucher.MaxUses, voucher.UsedCount, voucher.ExpiresAt,
            voucher.IsActive, voucher.CreatedAt);
    }
}

public class DeleteVoucherHandler : IRequestHandler<DeleteVoucherCommand>
{
    private readonly IUnitOfWork _uow;
    public DeleteVoucherHandler(IUnitOfWork uow) => _uow = uow;

    public async Task Handle(DeleteVoucherCommand req, CancellationToken ct)
    {
        var voucher = await _uow.Vouchers.GetByIdAsync(req.VoucherId, ct)
            ?? throw new NotFoundException("Voucher", req.VoucherId);

        if (!await _uow.Courses.IsInstructorOfCourseAsync(req.InstructorId, voucher.CourseId, ct))
            throw new ForbiddenException();

        _uow.Vouchers.Remove(voucher);
        await _uow.SaveChangesAsync(ct);
    }
}
