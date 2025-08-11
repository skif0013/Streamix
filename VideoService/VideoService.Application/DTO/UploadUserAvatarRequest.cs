using Microsoft.AspNetCore.Http;

namespace VideoService.Application.DTO;

public class UploadUserAvatarRequest
{
    public Guid UserId { get; set; }
    public IFormFile File { get; set; }
}