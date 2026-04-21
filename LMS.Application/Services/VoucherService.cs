using LMS.Application.Services.Interfaces;
using LMS.Domain.DTOs.Vouchers;

namespace LMS.Application.Services;

public class VoucherService : IVoucherService
{
    public Task<VoucherDto> CreateVoucherAsync(Guid instructorId, CreateVoucherDto dto, CancellationToken ct = default)
        => throw new NotImplementedException();

    public Task<VoucherDto> UpdateVoucherAsync(Guid voucherId, Guid instructorId, UpdateVoucherDto dto, CancellationToken ct = default)
        => throw new NotImplementedException();

    public Task DeleteVoucherAsync(Guid voucherId, Guid instructorId, CancellationToken ct = default)
        => throw new NotImplementedException();

    public Task<IEnumerable<VoucherDto>> GetCourseVouchersAsync(Guid courseId, Guid instructorId, CancellationToken ct = default)
        => throw new NotImplementedException();

    public Task<VoucherDto> ValidateVoucherAsync(string code, Guid courseId, CancellationToken ct = default)
        => throw new NotImplementedException();

    public Task<decimal> CalculateDiscountAsync(string code, Guid courseId, CancellationToken ct = default)
        => throw new NotImplementedException();
}
