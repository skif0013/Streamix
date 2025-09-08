namespace VideoService.Application.Model;

public class Avatar
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string FileName { get; set; }
    public long FileSize { get; set; }
    public DateTime UploadDate { get; set; }
    
    
    public string FileUrl { get; set; }
    
}