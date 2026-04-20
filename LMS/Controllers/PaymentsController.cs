using LMS.Application.Services.Interfaces;
using LMS.Common.Constants;
using LMS.DTOs.Enrollments;
using LMS.DTOs.Payments;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LMS.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class PaymentsController : ControllerBase
{
    private readonly IPaymentService _paymentService;

    public PaymentsController(IPaymentService paymentService) => _paymentService = paymentService;

    private Guid GetUserId() => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    // POST api/v1/payments/intent  [Student]
    [HttpPost("intent")]
    [Authorize(Roles = Roles.Student)]
    [ProducesResponseType(typeof(PaymentIntentResponseDto), 200)]
    public async Task<IActionResult> CreatePaymentIntent([FromBody] CreatePaymentIntentDto dto, CancellationToken ct)
    {
        var result = await _paymentService.CreatePaymentIntentAsync(GetUserId(), dto, ct);
        return Ok(result);
    }

    // POST api/v1/payments/confirm  [Student]
    [HttpPost("confirm")]
    [Authorize(Roles = Roles.Student)]
    [ProducesResponseType(typeof(EnrollmentDto), 200)]
    public async Task<IActionResult> ConfirmPayment([FromBody] ConfirmPaymentDto dto, CancellationToken ct)
    {
        var result = await _paymentService.ConfirmPaymentAndEnrollAsync(GetUserId(), dto, ct);
        return Ok(result);
    }

    // POST api/v1/payments/apply-voucher  [Student]
    [HttpPost("apply-voucher")]
    [Authorize(Roles = Roles.Student)]
    [ProducesResponseType(typeof(PaymentIntentResponseDto), 200)]
    public async Task<IActionResult> ApplyVoucher([FromBody] ApplyVoucherDto dto, CancellationToken ct)
    {
        var result = await _paymentService.ApplyVoucherToIntentAsync(GetUserId(), dto, ct);
        return Ok(result);
    }

    // POST api/v1/payments/webhook  [Stripe — no auth]
    [HttpPost("webhook")]
    [AllowAnonymous]
    [ProducesResponseType(200)]
    public async Task<IActionResult> StripeWebhook(CancellationToken ct)
    {
        var payload = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync(ct);
        var signature = Request.Headers["Stripe-Signature"].ToString();
        await _paymentService.HandleStripeWebhookAsync(payload, signature, ct);
        return Ok();
    }

    // GET api/v1/payments/courses/{courseId}/revenue  [Instructor/Admin]
    [HttpGet("courses/{courseId:guid}/revenue")]
    [Authorize(Roles = $"{Roles.Instructor},{Roles.Admin}")]
    [ProducesResponseType(typeof(decimal), 200)]
    public async Task<IActionResult> GetCourseRevenue(Guid courseId, CancellationToken ct)
    {
        var result = await _paymentService.GetCourseRevenueAsync(courseId, GetUserId(), ct);
        return Ok(new { Revenue = result });
    }
}
