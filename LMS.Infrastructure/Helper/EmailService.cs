using LMS.Domain.Common;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace LMS.Infrastructure.Services;

public class EmailService
{
    private readonly EmailSettings _settings;

    public EmailService(IOptions<EmailSettings> settings) => _settings = settings.Value;

    public async Task SendOtpAsync(string toEmail, string otp, CancellationToken ct = default)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(_settings.DisplayName, _settings.From));
        message.To.Add(new MailboxAddress("", toEmail));
        message.Subject = "Your verification code";

        message.Body = new TextPart("plain")
        {
            Text = $"""
                Your IAM THE SEA verification code is:

                {otp}

                This code expires in 10 minutes.
                If you did not request this, ignore this email.
                """
        };

        using var client = new SmtpClient();
        await client.ConnectAsync(_settings.SmtpHost, _settings.SmtpPort, SecureSocketOptions.StartTls, ct);
        await client.AuthenticateAsync(_settings.From, _settings.AppPassword, ct);
        await client.SendAsync(message, ct);
        await client.DisconnectAsync(true, ct);
    }
}