namespace VideoService.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class FilesController : ControllerBase
{
    public FilesController()
    {
        
    }
    
    [HttpGet("file")]
    public async Task<Result<string>> UploadFile()
    {
        return Result<string>.Success(string.Empty);
    }
}