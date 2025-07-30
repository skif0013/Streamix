using EmailService.Inrerfaces;
using EmailService.Model;
using System.Net.Mail;
using System.Text;
using Microsoft.Extensions.Options;
using Sprache;


namespace EmailService.Services;

public class EmailService : IEmailService
{
    private readonly ISmtpClientFactory _smtpClientFactory;
    private readonly IConfiguration _configuration;
    private readonly EmailSettings _settings;

    public EmailService(IConfiguration configuration, ISmtpClientFactory smtpClientFactory,IOptions<EmailSettings> emailSettings)
    {
        _settings = emailSettings.Value;
        _smtpClientFactory = smtpClientFactory;
        _configuration = configuration;
    }
    
    public Task SendEmailAsync(EmailRequest request)
    {
        var client = _smtpClientFactory.CreateClient();

        var senderEmail = Environment.GetEnvironmentVariable("SENDER_EMAIL");
            
        var mailMessage = new MailMessage
        {
            From = new MailAddress(senderEmail, senderEmail),
            Subject = request.Subject,
            Body = request.Body,
            IsBodyHtml = true,
        };
        
        mailMessage.To.Add(request.To);

        var bodyBuilder = new StringBuilder()
            .AppendLine(request.Body)
            .AppendLine();
        
        mailMessage.Body = bodyBuilder.ToString();
        client.Send(mailMessage);


        return Task.CompletedTask;
    }

    public async Task SendVerificationCodeAsync(EmailVerification verification)
    {
        var subject = _configuration["EmailVerification:RegisterVerification:Subject"];
        var body = _configuration["EmailVerification:RegisterVerification:Body"]
            .Replace("{code}", verification.Code);

        using var mailMessage = new MailMessage
        {
            From = new MailAddress(_settings.Username),
            Subject = subject,
            Body = body,
            IsBodyHtml = true,
            To = { verification.To }
        };

        using var client = _smtpClientFactory.CreateClient();
        await client.SendMailAsync(mailMessage);
    }
}
