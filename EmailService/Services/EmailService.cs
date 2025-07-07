using EmailService.Inrerfaces;
using EmailService.Model;
using System.Net;
using System.Net.Mail;
using System.Text;


namespace EmailService.Services;

public class EmailService : IEmailService
{
    private readonly ILogger<EmailService> _logger;
    
    private readonly IConfiguration _configuration;
    

    public EmailService(ILogger<EmailService> logger, IConfiguration configuration)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public Task SendEmailAsync(EmailRequest request)
    {
        try
        {
            _logger.LogInformation("Preparing SMTP client");

           
            var email = _configuration["SmtpSettings:SenderEmail"];
            var password = _configuration["SmtpSettings:EmailPassword"];

            using var client = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(email, password),
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Timeout = 30000
            };

           var senderEmail = _configuration["SmtpSettings:SenderEmail"];
            
            var mailMessage = new MailMessage
            {
                From = new MailAddress(senderEmail), 
                Subject = request.Subject,
                IsBodyHtml = true
            };

            mailMessage.To.Add(request.To);

            var bodyBuilder = new StringBuilder();
            bodyBuilder.AppendLine(request.Body);
            bodyBuilder.AppendLine();
            bodyBuilder.AppendLine(request.AdditionalText);

            mailMessage.Body = bodyBuilder.ToString();

            _logger.LogInformation("Sending email to {To}", request.To);
            client.Send(mailMessage);
            _logger.LogInformation("Email sent successfully to {To}", request.To);
        }
        catch (SmtpException smtpEx)
        {
            _logger.LogError(smtpEx, "SMTP error while sending email to {To}", request.To);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "General error while sending email to {To}", request.To);
        }

        return Task.CompletedTask;
    }
}