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
    public async Task<IActionResult> Send([FromBody] EmailRequest request)
    {
        await _emailService.SendEmailAsync(request);
        return Ok("Email sent");
    }

    [HttpPost("verify")]
    public async Task<IActionResult> SendVerification([FromBody] EmailVerification request)
    {
        await _emailService.SendVerificationCodeAsync(request);
            return Ok("Verification code sent");
    }
}