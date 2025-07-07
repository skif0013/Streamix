namespace EmailService.Model;

public class EmailRequest
{
    public string To { get; set; } = default!;
    
    public string Subject { get; set; } = default!;
    
    public string Body { get; set; } = default!;
    
    public string AdditionalText { get; set; } = default!;
}