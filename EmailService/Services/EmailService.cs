using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System.Net.Mail;
using System.IO;
using System.Text;
using System.Threading;
using EmailService.Inrerfaces;

namespace EmailService.Services;

public class EmailService : IEmailService
{
    private readonly GmailService _gmailService;

    public EmailService()
    {
        _gmailService = InitializeEmailService();
    }

    private GmailService InitializeEmailService()
    {
        using var stream = new FileStream("client_secret.json", FileMode.Open, FileAccess.Read); 
        
        string credPath = "token.json";
        
        var credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
            GoogleClientSecrets.FromStream(stream).Secrets,
            new[] { GmailService.Scope.GmailSend },
            "user",
            CancellationToken.None,
            new FileDataStore(credPath, true)).Result;

        return new GmailService(new BaseClientService.Initializer()
        {
            HttpClientInitializer = credential,
            ApplicationName = "Gmail API Sender"
        });
    }

    public async Task SendEmailAsync(string email, string subject, string message)
    {
        var mimeMessage = new StringBuilder();
        mimeMessage.AppendLine("From: danik.medvedev99@gmail.com");
        mimeMessage.AppendLine($"To: {email}");
        mimeMessage.AppendLine($"Subject: {subject}");
        mimeMessage.AppendLine("Content-Type: text/plain; charset=utf-8");
        mimeMessage.AppendLine();
        mimeMessage.AppendLine(message);

        var bytes = Encoding.UTF8.GetBytes(mimeMessage.ToString());
        var base64Url = Convert.ToBase64String(bytes)
            .Replace('+', '-')
            .Replace('/', '_')
            .Replace("=", "");

        var gmailMessage = new Google.Apis.Gmail.v1.Data.Message
        {
            Raw = base64Url
        };

        await _gmailService.Users.Messages.Send(gmailMessage, "me").ExecuteAsync();
    }

}