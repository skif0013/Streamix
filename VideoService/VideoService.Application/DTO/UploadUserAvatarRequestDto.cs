using Microsoft.AspNetCore.Http;

namespace VideoService.Application.DTO;

public class UploadUserAvatarRequestDto
{
    public Guid UserId { get; set; }

    public string validate { get; set; }
    public IFormFile File { get; set; }
}

public class UploadUserAvatarValidator : AbstractValidator<UploadUserAvatarRequestDto>
{
    public UploadUserAvatarValidator()
    {
         /*RuleFor(x => x.UserId).NotNull().WithMessage("UserId is required");
        
        
        RuleFor(x => x.File)
            .NotNull().WithMessage("File is required")
            .Must(file => file.Length < 0)
            .WithMessage("File must not be empty")
            .Must(file => file.Length <= 1 * 1024)
            .WithMessage("File size must not exceed 5 MB");
        
        
        RuleFor(x => x.validate)
            .NotNull().WithMessage("validate is required__52");*/
    }
}