using System.Net.Mail;

namespace EmailService.Inrerfaces;

public interface ISmtpClientFactory
{
   SmtpClient  CreateClient();
}