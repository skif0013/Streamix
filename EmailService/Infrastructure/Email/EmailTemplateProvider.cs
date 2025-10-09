    using EmailService.Inrerfaces;

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
                   ?? $"Your email confirmation code: {code}"; 
        }

        public string GetVerificationSubject()
        {
            return _config["EmailTemplates:Verification:Subject"] 
                   ?? "Email confirmation code"; 
        }
    }