using System.Net.Mail;
using EmailService.Model;

namespace EmailService.Inrerfaces;

public interface IEmailBuilder
{
        MailMessage Build(EmailRequest request);
        MailMessage BuildVerificationEmail(EmailVerification request);
}