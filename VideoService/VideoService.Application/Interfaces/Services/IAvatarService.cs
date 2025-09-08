using VideoService.Application.Model;

namespace VideoService.Application.Interfaces.Services;

public interface IAvatarService
{
    Task<Result<Avatar>> UploadAvatarAsync(UploadUserAvatarRequestDto request, Guid userId);
    Task<Result<string>> UpdateAvatarAsync(Guid userId, string avatarUrl);
    Task<Result<string>> RemoveAvatarAsync(Guid userId);
    Task<Result<string>> GetAvatarAsync(Guid userId);
}