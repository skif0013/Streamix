using UserService.Infrastructure.Interfaces.Services;

namespace UserService.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
    private readonly IUserService _userService;
    private readonly ITokenService _tokenService;
    
        public UsersController(IUserService userService, ITokenService tokenService)
        {
            _userService = userService;
            _tokenService = tokenService;
        }
        [HttpPost("register")]
        public async Task<Result<string>> RegisterAsync(UserRequestDTO request)
        {
            var result = await _userService.RegisterAsync(request);
            return result;
        }

        [HttpPost("login")]
        public async Task<Result<AuthResponse>> LoginAsync(AuthRequest request)
        {
            var result = await _userService.AuthenticateAsync(request);
            return result;
        }

        [HttpPost("ForgotPassword")]
        public async Task<Result<string>> ForgotPasswordAsync(ForgotPasswordRequest request)
        {
            var result = await _userService.ForgotPasswordAsync(request);
            return result;
        }

        [HttpPost("ResetPassword")]
        public async Task<Result<string>> ResetPasswordAsync(ResetPsswordDto request, [FromHeader] string token)
        {
            var result = await _userService.ResetPasswordAsync(request, token);
            return result;
        }

        [HttpPost("ConfirmEmail")]
        public async Task<Result<string>> ConfimEmailAsync(string email, [FromHeader] string token)
        {
            var result = await _userService.ConfirmEmailAsync(email, token);
            return result;
        }

        [HttpPost("UploadTokens")]
        public async Task<Result<RefreshTokenResponseDto>> UploadTokensAsync(string refreshToken)
        {
            var result = await _tokenService.UploadTokensAsync(refreshToken);
            return result;
        }
    }
}
