using Microsoft.AspNetCore.Http.Extensions;

namespace VideoService.Application.Model;

public class Video
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; } 
    public string Title { get; set; }
    public string? Description { get; set; }
    public string FileName { get; set; }
    public string FileUrl { get; set; }
    public long FileSize { get; set; }
    public DateTime UploadDate { get; set; } = DateTime.UtcNow;
    public string StoragePath { get; set; } 
}
