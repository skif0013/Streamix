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

    [HttpGet()]
    public async Task<Result<List<Video>>> GetUserVideos()
    {
        var userId = Guid.Parse(User.FindFirst("userId")?.Value);
        return await _videoService.GetUserVideos(userId);
    }

    [HttpDelete("")]
    public async Task<Result<string>> DeleteVideo(Guid videoId)
    {
        var userId = Guid.Parse(User.FindFirst("userId")?.Value);
        return await _videoService.DeleteVideo(videoId, userId);
    }

    [HttpPost()]
    public async Task<Result<Video>> UploadVideo(UploadUserVideo request)
    {
        var userId = Guid.Parse(User.FindFirst("userId")?.Value);
        return   await _videoService.UploadVideo(request,userId);
    }
}