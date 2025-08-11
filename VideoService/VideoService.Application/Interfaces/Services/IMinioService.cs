using VideoService.Application.DTO;

namespace VideoService.Application.Interfaces.Services;

public interface IMinioService
{ 
    Task<Result<string>> CreateBucket(string bucketName);
    Task<Result<string>> DeleteBucket(string bucketName);
    Task<Result<string>> UploadUserAvatars(UploadUserAvatarRequest request);
    Task<Result<string>> DeleteObj(string bucketName, string fileName);
}