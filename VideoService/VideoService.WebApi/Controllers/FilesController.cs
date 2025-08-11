using Minio.DataModel.Args;
using VideoService.Application.DTO;
using VideoService.Application.Interfaces.Services;

namespace VideoService.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class FilesController : ControllerBase
{
    private readonly IMinioService _minioService;
    public FilesController(IMinioService minioService)
    {
        _minioService = minioService;
    }
    
    [HttpPost("CreateBucket")]
    public async Task<Result<string>> CreateBucket(string bucketName)
    {
        var result = await _minioService.CreateBucket(bucketName);
        return result;
    }
    
    [HttpPost("DeleteBucket")]
    public async Task<Result<string>> DeleteBucket(string bucketName)
    {
        var result = await _minioService.DeleteBucket(bucketName);
        return result;
    }

    [HttpPost("UploadPhoto")]
    [Consumes("multipart/form-data")]
    public async Task<Result<string>> UploadUserAvatars([FromForm]UploadUserAvatarRequest request)
    {
        var result = await _minioService.UploadUserAvatars(request);
        return result;
    }
    
    [HttpPost("DeleteObj")]
    public async Task<Result<string>> DeleteObj(string bucketName, string fileName)
    {
        var result = await _minioService.DeleteObj(bucketName, fileName);
        return result;
    }
}