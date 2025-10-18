using VideoService.Application.DTO;
using VideoService.Application.Model;
using VideoService.Core.Results;

namespace VideoService.Application.Services;

public class AvatarService : IAvatarService
{
    private readonly ApplicationDbContext _context;
    private readonly IMinioService _minioService;
    
    private const string AvatarBucket = "user-photos";

    public AvatarService(ApplicationDbContext context, IMinioService minioService)
    {
        _minioService = minioService;
        _context = context;
    }
     public async Task<Result<Avatar>> UploadAvatarAsync(UploadUserAvatarRequestDto request, Guid userId)
    {
        var uploadResult = await _minioService.UploadUserAvatar(request);

        var existingAvatar = await _context.Avatars.FirstOrDefaultAsync(a => a.UserId == userId);
        if (existingAvatar != null)
        {
            if (!string.IsNullOrEmpty(existingAvatar.FileUrl))
            { 
                var fileName = existingAvatar.FileUrl.Split('/', StringSplitOptions.RemoveEmptyEntries)[^1]; 
                await _minioService.DeleteObj(AvatarBucket, fileName);
            }
            _context.Avatars.Remove(existingAvatar);
            await _context.SaveChangesAsync();
        }   

        var avatar = new Avatar
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            FileName = request.File.FileName,
            FileSize = request.File.Length,
            UploadDate = DateTime.UtcNow,
            FileUrl = uploadResult.Data
        };

        await _context.Avatars.AddAsync(avatar);
        await _context.SaveChangesAsync();

        return Result<Avatar>.Success(avatar);
    }

    public async Task<Result<string>> UpdateAvatarAsync(Guid userId, string avatarUrl)
    {
        var avatar = await _context.Avatars.FirstOrDefaultAsync(a => a.UserId == userId);
        if (avatar == null)
            return Result<string>.Failure("Avatar not found");

        avatar.FileUrl = avatarUrl;
        avatar.FileName = avatarUrl;
        await _context.SaveChangesAsync();
        return Result<string>.Success(avatarUrl);
    }

    public async Task<Result<string>> RemoveAvatarAsync(Guid userId)
    {
        var avatar = await _context.Avatars.FirstOrDefaultAsync(a => a.UserId == userId);
        if (avatar == null)
            return Result<string>.Failure("Avatar not found");

        if (!string.IsNullOrEmpty(avatar.FileUrl))
        { 
            var fileName = avatar.FileUrl.Split('/', StringSplitOptions.RemoveEmptyEntries)[^1];
            await _minioService.DeleteObj(AvatarBucket, fileName);
        }

        _context.Avatars.Remove(avatar);
        await _context.SaveChangesAsync();
        return Result<string>.Success("Avatar removed");
    }

    public async Task<Result<string>> GetAvatarAsync(Guid userId)
    {
        var avatar = await _context.Avatars.FirstOrDefaultAsync(a => a.UserId == userId);
        if (avatar == null)
            return Result<string>.Failure("Avatar not found");

        return Result<string>.Success(avatar.FileUrl ?? avatar.FileName);
    }
}