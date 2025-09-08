using VideoService.Application.DTO;
using VideoService.Application.Interfaces.Services;
using VideoService.Application.Model;

namespace VideoService.Controllers;

[ApiController]
[Route("api/videos")]
public class VideosController : ControllerBase
{
    private readonly IVideoService _videoService;

    public VideosController(IVideoService videoService)
    {
        _videoService = videoService;
    }

    [HttpGet("user/{userId}")]
    public async Task<Result<List<Video>>> GetUserVideos(Guid userId)
    {
        return await _videoService.GetUserVideos(userId);
    }

    [HttpDelete("{videoId}")]
    public async Task<Result<string>> DeleteVideo(Guid videoId, Guid userId)
    {
        return await _videoService.DeleteVideo(videoId, userId);
    }

    [HttpPost("upload")]
    public async Task<Result<Video>> UploadVideo(UploadUserVideo request, Guid userId)
    {
        var userIdClaim = User.FindFirst("userId")?.Value;
        
        userId = Guid.Parse(userIdClaim);
        
        return   await _videoService.UploadVideo(request,userId);
    }
}