using MediatR;

namespace LMS.Application.Command.Voucher;

public record DeleteVoucherCommand(Guid VoucherId, Guid InstructorId) : IRequest;
