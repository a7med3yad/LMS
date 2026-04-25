using LMS.Application.DTOs.Vouchers;
using MediatR;

namespace LMS.Application.Command.Voucher;

public record UpdateVoucherCommand(Guid VoucherId, Guid InstructorId, UpdateVoucherDto Dto) : IRequest<VoucherDto>;
