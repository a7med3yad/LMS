using LMS.Application.DTOs.Vouchers;
using LMS.Domain.Models;
using MediatR;

namespace LMS.Application.Command.Voucher;

public record CreateVoucherCommand(Guid InstructorId, CreateVoucherDto Dto) : IRequest<VoucherDto>;
