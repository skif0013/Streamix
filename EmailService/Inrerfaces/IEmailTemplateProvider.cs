

namespace EmailService.Inrerfaces;

public interface IEmailTemplateProvider
{
    string GetVerificationBody(string code); 
    string GetVerificationSubject();    
}
