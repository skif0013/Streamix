using EmailService.Model;

namespace EmailService.Inrerfaces;

public interface IEmailService
{
    Task SendEmailAsync(EmailRequest request);
    Task SendVerificationCodeAsync(EmailVerification request);
}