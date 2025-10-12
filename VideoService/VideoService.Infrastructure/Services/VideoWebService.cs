using VideoService.Application.DTO;
using VideoService.Application.Model;
using VideoService.Core.Results;

namespace VideoService.Infrastructure.Services;

public class VideoWebService : IVideoService
{
    private readonly IMinioService _minioService;
    private readonly ApplicationDbContext _context;

    public VideoWebService(IMinioService minioService, ApplicationDbContext context)
    {
        _minioService = minioService;
        _context = context;
    }

    public async Task<Result<Video>> UploadVideo(UploadUserVideo request, Guid userId)
    {
            var objectName = $"{userId}/{Guid.NewGuid()}_{request.videoFile.FileName}";
            
            var uploadResult = await _minioService.UploadVideo(request, userId);
            
            var video = new Video
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Title = request.Title,
                Description = request.Description,
                FileName = request.videoFile.FileName,
                FileUrl = uploadResult.Data,
                FileSize = request.videoFile.Length,
                StoragePath = objectName,
                UploadDate = DateTime.UtcNow
            };

            await _context.AddAsync(video);
            await _context.SaveChangesAsync();
            
            return Result<Video>.Success(video);
    }
    
    public async Task<Result<List<Video>>> GetUserVideos(Guid userId)
    { 
        var videos = await _context.Videos
            .Where(v => v.UserId == userId)
            .ToListAsync();
            
        return Result<List<Video>>.Success(videos);
    }

    public async Task<Result<string>> DeleteVideo(Guid videoId, Guid userId)
    {
        var video = await _context.Videos
            .FirstOrDefaultAsync(v => v.Id == videoId && v.UserId == userId);
            
        if (video == null)
            return Result<string>.Failure("Video not found or access denied");

     
        var deleteResult = await _minioService.DeleteVideo(video.StoragePath);
        
        if (deleteResult.IsError)
            return deleteResult;
        
        _context.Videos.Remove(video);
        await _context.SaveChangesAsync();
        
        return Result<string>.Success("Video deleted successfully");
    }
}
  