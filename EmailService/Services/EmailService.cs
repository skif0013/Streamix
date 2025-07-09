using EmailService.Inrerfaces;
using EmailService.Model;
using System.Net.Mail;
using System.Text;


namespace EmailService.Services;

public class EmailService : IEmailService
{
    private readonly ISmtpClientFactory _smtpClientFactory;
    private readonly IConfiguration _configuration;
    

    public EmailService(ILogger<EmailService> logger, IConfiguration configuration, ISmtpClientFactory smtpClientFactory)
    {
        _smtpClientFactory = smtpClientFactory;
        _configuration = configuration;
    }
    
    public Task SendEmailAsync(EmailRequest request)
    {
        var client = _smtpClientFactory.CreateClient();
        
        var senderEmail = _configuration["SmtpSettings:SenderEmail"];
        
        var mailMessage = new MailMessage
        {
            From = new MailAddress(senderEmail, senderEmail),
            Subject = request.Subject,
            Body = request.Body,
            IsBodyHtml = true
        };
        
        mailMessage.To.Add(request.To);
        
        var bodyBuilder = new StringBuilder()
            .AppendLine(request.Body)
            .AppendLine()
            .AppendLine(request.AdditionalText);

        mailMessage.Body = bodyBuilder.ToString();
        client.Send(mailMessage);


        return Task.CompletedTask;
    }

    public Task SendVereficationCodeAsync(EmailVerification verification)
    {
        var client = _smtpClientFactory.CreateClient();
        
        var senderEmail = _configuration["SmtpSettings:SenderEmail"];
        
        var subject = _configuration["EmailVerification:RegisterVerification:Subject"];
        var bodyTemplate = _configuration["EmailVerification:RegisterVerification:Body"];
        
        var mailMessage = new MailMessage
        {
            From = new MailAddress(senderEmail, senderEmail),
            Subject = subject,
            Body = bodyTemplate,
            IsBodyHtml = true
        };
        
        mailMessage.To.Add(verification.To);
        
        client.Send(mailMessage);
        
        return Task.CompletedTask;
    }
}
