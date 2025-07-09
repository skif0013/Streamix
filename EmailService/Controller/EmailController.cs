using EmailService.Inrerfaces;
using EmailService.Model;
using Microsoft.AspNetCore.Mvc;

namespace EmailService.Controller;

[ApiController]
[Route("api/[controller]")]
public class EmailController : ControllerBase
{
    private readonly IEmailService _emailService;

    public EmailController(IEmailService emailService)
    {
        _emailService = emailService;
    }

    [HttpPost("send")]
    public async Task<IActionResult> SendEmail([FromBody] EmailRequest request)
    {
        await _emailService.SendEmailAsync(request);
        return Ok("Email sent");
    }

    [HttpPost("confirm-email")]
    public async Task<IActionResult> ConfirmEmail([FromBody] EmailVerification verify)
    {
        await _emailService.SendVereficationCodeAsync(verify);
        return Ok("Email verification sent");
    }
}