using VideoService.Application.Model;

namespace VideoService.Application.Interfaces.Services;

public interface IVideoService
{
    Task<Result<Video>> UploadVideo(UploadUserVideo request, Guid userId);
    Task<Result<List<Video>>> GetUserVideos(Guid userId);
    Task<Result<string>> DeleteVideo(Guid videoId, Guid userId);
}
