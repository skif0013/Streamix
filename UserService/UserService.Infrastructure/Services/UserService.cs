using UserService.Application.Interfaces.Services;
using UserService.Application.DTO;
using UserService.Core.Results;
using UserService.Infrastructure.Interfaces.Services;

namespace UserService.Infrastructure.Services;

public class UserService : IUserService
{
    private readonly UserManager<UserIdentity> _userManager;
    private readonly RoleManager<RoleIdentity> _roleManager;
    private readonly ITokenService _tokenService;

    public UserService(
        UserManager<UserIdentity> userManager,
        RoleManager<RoleIdentity> roleManager,
        ITokenService tokenService
    )
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _tokenService = tokenService;
    }
    
    public async Task<Result<string>> RegisterAsync(UserRequestDTO request)
    {
        var user = new UserIdentity
        {
            UserName = request.NickName,
            Email = request.Email,
        };
        
        var result = await _userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            return Result<string>.Failure($"Error creating user: {errors}");
        }

        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        Console.WriteLine(token);
        
        /*using var client = new HttpClient();
        string url = "http://emailservice:5003/api/Email/verify";
        string json = $@"{{
            ""to"": ""{request.Email}"",
            ""code"": ""{token}""
        }}";*/

        //var content = new StringContent(json, Encoding.UTF8, "application/json");

        //var response = await client.PostAsync(url, content);
        
        return Result<string>.Success("user created successfully");
    }
    
    public async Task<Result<AuthResponse>> AuthenticateAsync(AuthRequest request)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
        if (user == null)
        {
            return Result<AuthResponse>.Failure("Invalid Email");
        }

        var isEmailConfirmed = await _userManager.IsEmailConfirmedAsync(user);
        if (!isEmailConfirmed)
        {
            return Result<AuthResponse>.Failure("Email not confirmed");
        }

        var isPasswordCorrect = await _userManager.CheckPasswordAsync(user, request.Password);
        if (!isPasswordCorrect)
        {
            return Result<AuthResponse>.Failure("Invalid Password");
        }
        
        var tokens = await _tokenService.CreateRefreshTokenAsync(user);
        
        return Result<AuthResponse>.Success(new AuthResponse
        {
            Username = user.UserName,
            Email = user.Email,
            Token = tokens.AccessToken,
            RefreshToken = tokens.RefreshToken
        });
    }
    
    public async Task<Result<string>> ForgotPasswordAsync(ForgotPasswordRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null)
        {
            return Result<string>.Failure($"{request.Email} - this email address is not registered");
        }
        
        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        Console.WriteLine(token);
        
        return Result<string>.Success("Password reset successfully");
    }

    public async Task<Result<string>> ResetPasswordAsync(ResetPsswordDto request, string token)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null)
        {
            return Result<string>.Failure($"{request.Email} - this email address is not registered");
        }
        
        var resetPassResult = await _userManager.ResetPasswordAsync(user, token, request.NewPassword!);

        if(!resetPassResult.Succeeded)
        {
            var errors = resetPassResult.Errors.Select(e => e.Description);
            return Result<string>.Failure(errors.ToString()!);
        }
        
        return Result<string>.Success("Password reset successfully");
    }

    public async Task<Result<string>> ConfirmEmailAsync(string email, string token)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
        {
            return Result<string>.Failure("Invalid email address");
        }
        await _userManager.ConfirmEmailAsync(user, token);
        
        var roleExists = _roleManager.FindByNameAsync("User".ToString());
        if (!roleExists.IsCompletedSuccessfully)
        {
            Console.WriteLine("Role doesn't exist");
        }

        await _userManager.AddToRoleAsync(user, roleExists.Result.Name.ToString());
        
        return Result<string>.Success("Email confirmed");
    }
}