using EmailService.Inrerfaces;
using EmailService.Model;

namespace EmailService.Services;

public class EmailTemplateProvider : IEmailTemplateProvider
{
    private readonly IConfiguration _config;

    public EmailTemplateProvider(IConfiguration config)
    {
        _config = config;
    }

    public string GetVerificationBody(string code)
    {
        string template = _config["EmailTemplates:Verification:Body"];
        return template?.Replace("{code}", code) 
               ?? $"Ваш код подтверждения: {code}"; // fallback
    }

    public string GetVerificationSubject()
    {
        return _config["EmailTemplates:Verification:Subject"] 
               ?? "Подтверждение email"; // fallback
    }
}