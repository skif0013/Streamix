using Microsoft.AspNetCore.Http;

namespace VideoService.Application.DTO;

public class UploadUserVideo
{
    public IFormFile videoFile { get; set; }
    
    public string Title { get; set; }
    
    public string? Description { get; set; }    
}