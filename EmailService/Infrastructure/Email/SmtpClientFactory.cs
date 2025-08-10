using System.Net;
using System.Net.Mail;
using EmailService.Inrerfaces;
using EmailService.Model;
using Microsoft.Extensions.Options;

namespace EmailService.Services;

public class SmtpClientFactory : ISmtpClientFactory
{
    private readonly EmailSettings _settings;

    public SmtpClientFactory(IOptions<EmailSettings> emailSettings)
    {
        _settings = emailSettings.Value; 
    }

    public SmtpClient CreateClient()
    {
        return new SmtpClient(_settings.Host, _settings.Port)
        {
            EnableSsl = _settings.EnableSsl,
            Credentials = new NetworkCredential(_settings.Username, _settings.Password)
        };
    }
}