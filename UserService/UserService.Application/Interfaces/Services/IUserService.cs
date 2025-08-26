namespace UserService.Application.Interfaces.Services;

public interface IUserService
{
    Task<Result<string>> RegisterAsync(UserRequestDTO user);
    Task<Result<AuthResponse>> AuthenticateAsync(AuthRequest request);
    Task<Result<string>> ForgotPasswordAsync(ForgotPasswordRequest request);
    Task<Result<string>> ResetPasswordAsync(ResetPsswordDto request, string token);
    Task<Result<string>> ConfirmEmailAsync(string email, string token);
}