using System.Net;
using System.Net.Mail;
using EmailService.Inrerfaces;

namespace EmailService.Services;

public class SmtpClientFactory : ISmtpClientFactory{
    
    private readonly IConfiguration _configuration;


    public SmtpClientFactory(IConfiguration configuration)
    {
        _configuration = configuration;
    }


    public SmtpClient CreateClient()
    {
        var email = _configuration["SmtpSettings:SenderEmail"];
        var password = _configuration["SmtpSettings:EmailPassword"];

        return new SmtpClient("smtp.gmail.com", 587)
        {
            EnableSsl = true,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential(email, password),
            DeliveryMethod = SmtpDeliveryMethod.Network,
            Timeout = 30000
        };
    }
}