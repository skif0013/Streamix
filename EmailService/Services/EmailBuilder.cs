using System.Net.Mail;
using EmailService.Inrerfaces;
using EmailService.Model;
using Microsoft.Extensions.Options;

namespace EmailService.Services;

public class EmailBuilder : IEmailBuilder
{
    private readonly EmailSettings _settings;
    private readonly IEmailTemplateProvider _templateProvider;
    
    public EmailBuilder(
        IOptions<EmailSettings> emailSettings,
        IEmailTemplateProvider templateProvider)
    {
        _settings = emailSettings.Value;
        _templateProvider = templateProvider;
    }

    public MailMessage Build(EmailRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.To))
            throw new ArgumentException("Recipient email is required");

        return new MailMessage
        {
            From = new MailAddress(_settings.Username),
            Subject = request.Subject ?? string.Empty,
            Body = request.Body ?? string.Empty,
            IsBodyHtml = true,
            To = { request.To.Trim() }
        };
    }

    public MailMessage BuildVerificationEmail(EmailVerification request)
    {
        if (string.IsNullOrWhiteSpace(request.To))
            throw new ArgumentException("Recipient email is required");
        
        if (string.IsNullOrWhiteSpace(request.Code))
            throw new ArgumentException("Verification code is required");

        return new MailMessage
        {
            From = new MailAddress(_settings.Username),
            Subject = _templateProvider.GetVerificationSubject(),
            Body = _templateProvider.GetVerificationBody(request.Code),
            IsBodyHtml = true,
            To = { request.To.Trim() }
        };
    }
}