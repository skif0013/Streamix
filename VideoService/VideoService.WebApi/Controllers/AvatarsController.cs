using VideoService.Application.DTO;
using VideoService.Application.Interfaces.Services;
using VideoService.Application.Model;

namespace VideoService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AvatarsController : ControllerBase
{
    private readonly IAvatarService _avatarService;

    public AvatarsController(IAvatarService avatarService)
    {
        _avatarService = avatarService;
    }
    
    [HttpPost("upload")]
    [Consumes("multipart/form-data")]
    public async Task<Result<Avatar>> Upload([FromForm] UploadUserAvatarRequestDto request)
    {
        var userId = Guid.Parse(User.FindFirst("userId")?.Value);
        return await _avatarService.UploadAvatarAsync(request, userId);
    }
    
    [HttpGet()]
    public async Task<Result<string>> Get()
    {
        var userId = Guid.Parse(User.FindFirst("userId")?.Value);
        return await _avatarService.GetAvatarAsync(userId);
    }

    [HttpPut("")]
    public async Task<Result<string>> Update([FromBody] string avatarUrl)
    {
        var userId = Guid.Parse(User.FindFirst("userId")?.Value);
        return await _avatarService.UpdateAvatarAsync(userId, avatarUrl);
    }
    
    [HttpDelete("")]
    public async Task<Result<string>> Delete()
    {
        var userId = Guid.Parse(User.FindFirst("userId")?.Value);
        return await _avatarService.RemoveAvatarAsync(userId);
    }
}