using LMS.DTOs.Vouchers;

namespace LMS.Application.Services.Interfaces;

public interface IVoucherService
{
    Task<VoucherDto> CreateVoucherAsync(Guid instructorId, CreateVoucherDto dto, CancellationToken ct = default);

    Task<VoucherDto> UpdateVoucherAsync(Guid voucherId, Guid instructorId, UpdateVoucherDto dto, CancellationToken ct = default);

    Task DeleteVoucherAsync(Guid voucherId, Guid instructorId, CancellationToken ct = default);

    Task<IEnumerable<VoucherDto>> GetCourseVouchersAsync(Guid courseId, Guid instructorId, CancellationToken ct = default);

    Task<VoucherDto> ValidateVoucherAsync(string code, Guid courseId, CancellationToken ct = default);

    Task<decimal> CalculateDiscountAsync(string code, Guid courseId, CancellationToken ct = default);
}
