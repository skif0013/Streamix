using System.ComponentModel.DataAnnotations;

namespace EmailService.Model;

public class EmailSettings
{
    public string Host { get; set; }
    public int Port { get; set; } = 587;
    public string Username { get; set; }
    public string Password { get; set; }
    public bool EnableSsl { get; set; }
    
    public string SenderEmail { get; set; }
}