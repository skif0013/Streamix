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

    // POST api/avatars/upload?userId=...
    [HttpPost("upload")]
    [Consumes("multipart/form-data")]
    public async Task<Result<Avatar>> Upload([FromForm] UploadUserAvatarRequestDto request, [FromQuery] Guid userId)
    {
        
        
        var claim = User.FindFirst("userId")?.Value;
        Console.WriteLine($"Claim userId: {claim}");
        if (userId == Guid.Empty)
        {
            
            if (!string.IsNullOrWhiteSpace(claim) && Guid.TryParse(claim, out var parsed))
                userId = parsed;
        }

        if (userId == Guid.Empty)
            return Result<Avatar>.Failure("userId не передан");

        return await _avatarService.UploadAvatarAsync(request, userId);
    }

    // GET api/avatars/{userId}
    [HttpGet("{userId:guid}")]
    public async Task<Result<string>> Get(Guid userId)
    {
        if (userId == Guid.Empty)
            return Result<string>.Failure("userId пустой");
        return await _avatarService.GetAvatarAsync(userId);
    }

    // PUT api/avatars/{userId}
    // Тело запроса: просто строка ("https://.../avatar.png")
    [HttpPut("{userId:guid}")]
    public async Task<Result<string>> Update(Guid userId, [FromBody] string avatarUrl)
    {
        if (userId == Guid.Empty)
            return Result<string>.Failure("userId пустой");
        if (string.IsNullOrWhiteSpace(avatarUrl))
            return Result<string>.Failure("avatarUrl пустой");
        return await _avatarService.UpdateAvatarAsync(userId, avatarUrl);
    }

    // DELETE api/avatars/{userId}
    [HttpDelete("{userId:guid}")]
    public async Task<Result<string>> Delete(Guid userId)
    {
        if (userId == Guid.Empty)
            return Result<string>.Failure("userId пустой");
        return await _avatarService.RemoveAvatarAsync(userId);
    }
}