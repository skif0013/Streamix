using EmailService.Inrerfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace EmailService.Controller;

public class EmailController : ControllerBase
{
    private readonly IEmailService _emailService;

    public EmailController(IEmailService emailService)
    {
        _emailService = emailService;
    }

    [HttpPost("send")]
    public async Task<IActionResult> SendEmail([FromQuery] string email)
    {
        await _emailService.SendEmailAsync(email, "Test","I love u Yana \n from Daniel");
        
        return Ok("Email sent");
    }
}