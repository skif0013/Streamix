using VideoService.Application.DTO;

namespace VideoService.Application.Interfaces.Services;

public interface IMinioService
{ 
    Task<Result<string>> CreateBucket(string bucketName);
    Task<Result<string>> DeleteBucket(string bucketName);
    Task<Result<string>> UploadUserAvatar(UploadUserAvatarRequestDto requestDto);
    Task<Result<string>> DeleteObj(string bucketName, string fileName);
    
    Task<Result<string>> UploadVideo(UploadUserVideo request, Guid userId);
    Task<Result<string>> DeleteVideo(string objectName);
}