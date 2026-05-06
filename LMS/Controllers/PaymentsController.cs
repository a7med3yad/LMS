using LMS.Application.Command.Payment;
using LMS.Application.DTOs.Payments;
using LMS.Application.Query.Payment;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LMS.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class PaymentsController : ControllerBase
{
    private readonly ISender _sender;
    public PaymentsController(ISender sender) => _sender = sender;

    private Guid UserId() =>
        Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    [HttpPost]
    [Authorize(Roles = "Student")]
    public async Task<IActionResult> CreatePayment(CreatePaymentDto dto, CancellationToken ct)
        => Ok(await _sender.Send(new CreatePaymentCommand(UserId(), dto), ct));

    [HttpGet("my")]
    [Authorize(Roles = "Student")]
    public async Task<IActionResult> MyPayments(CancellationToken ct)
        => Ok(await _sender.Send(new GetMyPaymentsQuery(UserId()), ct));

    // webhook endpoint (no auth)
    [HttpPost("webhook/confirm")]
    [AllowAnonymous]
    public async Task<IActionResult> ConfirmPayment(Guid paymentId, string transactionId, CancellationToken ct)
    {
        await _sender.Send(new ConfirmPaymentCommand(paymentId, transactionId), ct);
        return Ok("Payment confirmed");
    }
}
