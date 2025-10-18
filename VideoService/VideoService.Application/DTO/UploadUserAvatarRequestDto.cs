namespace VideoService.Application.DTO;

public class UploadUserAvatarRequestDto
{
    public IFormFile File { get; set; }
}

public class UploadUserAvatarValidator : AbstractValidator<UploadUserAvatarRequestDto>
{
    public UploadUserAvatarValidator()
    {
        RuleFor(x => x.File)
            .NotNull().WithMessage("File is required")
            .Must(file => file.Length < 0)
            .WithMessage("File must not be empty")
            .Must(file => file.Length <= 1L * 1024 * 1024 * 1024)
            .WithMessage("File size must not exceed 5 MB");
    }
}