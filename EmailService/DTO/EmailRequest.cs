namespace EmailService.Model;

public class EmailRequest
{
    public string To { get; set; }
    
    public string Subject { get; set; } = default!;
    
    public string Body { get; set; } = default!;
}
