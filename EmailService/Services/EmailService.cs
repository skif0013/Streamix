using EmailService.Inrerfaces;
using EmailService.Model;
using System.Net.Mail;
using System.Text;
using Microsoft.Extensions.Options;



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
    
    public async Task SendEmailAsync(EmailRequest request)
    {
      using  var client = _smtpClientFactory.CreateClient();

      var senderEmail = Environment.GetEnvironmentVariable("SENDER_EMAIL");
            
      using var mailMessage = new MailMessage
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
        await client.SendMailAsync(mailMessage);
    }

    public async Task SendVerificationCodeAsync(EmailVerification verification)
    {
        var subject =  _configuration?["EmailVerification:RegisterVerification:Subject"] 
                      ?? "Код подтверждения";
    
        var bodyTemplate = _configuration?["EmailVerification:RegisterVerification:Body"] 
                           ?? "Ваш код подтверждения: {code}";
    
        var body = bodyTemplate.Replace("{code}", verification.Code);
        
        var senderEmail = _settings?.Username ?? Environment.GetEnvironmentVariable("SENDER_EMAIL") 
            ?? throw new InvalidOperationException("Не задан email отправителя");
        using var mailMessage = new MailMessage
        {
            From = new MailAddress(senderEmail),
            Subject = subject,
            Body = body,
            IsBodyHtml = true,
            To = { verification.To }
        };

        using var client = _smtpClientFactory.CreateClient();
        await client.SendMailAsync(mailMessage);
    }
}
