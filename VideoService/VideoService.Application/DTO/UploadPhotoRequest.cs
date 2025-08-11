using Microsoft.AspNetCore.Http;

namespace VideoService.Application.DTO;

public class UploadPhotoRequest
{ 
    public IFormFile File { get; set; }
    public string bucketName { get; set; }
}