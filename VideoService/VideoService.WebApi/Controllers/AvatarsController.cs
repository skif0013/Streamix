using VideoService.Application.DTO;
using VideoService.Application.Interfaces.Services;
using VideoService.Application.Model;

namespace VideoService.Controllers;

public class AvatarsController : ControllerBase
{
    private readonly IAvatarService _avatarService;
    

    public AvatarsController(IAvatarService avatarService)
    {
        _avatarService = avatarService;
    }

    [HttpPost("upload")]
    [Consumes("multipart/form-data")]
    public async Task<Result<Avatar>> UploadAvatar([FromForm] UploadUserAvatarRequestDto request, Guid userId)
    {
        var userIdClaim = User.FindFirst("userId")?.Value;
        
        userId = Guid.Parse(userIdClaim);
        
        return await _avatarService.UploadAvatarAsync(request,userId);
    }
}