using Microsoft.AspNetCore.Http;
using UserService.Application.Interfaces.Services;
using UserService.Application.DTO;
using UserService.Core.Results;
using UserService.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using UserService.Infrastructure.Interfaces.Services;

namespace UserService.Infrastructure.Services;

public class UserService : IUserService
{
    private readonly UserManager<UserIdentity> _userManager;
    //private readonly SignInManager<UserIdentity> _signInManager;
    private readonly ITokenService _tokenService;

    public UserService(
        UserManager<UserIdentity> userManager,
        //SignInManager<UserIdentity> signInManager
        ITokenService tokenService
    )
    {
        _userManager = userManager;
        //_signInManager = signInManager;
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

        return Result<string>.Success("user created successfully");
    }

    public async Task<Result<AuthResponse>> AuthenticateAsync(AuthRequest request)
    {
        var user = await _userManager.Users.SingleOrDefaultAsync(u => u.Email == request.Email);
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
        
        var token = await _tokenService.CreateTokenAsync(user);

        return Result<AuthResponse>.Success(new AuthResponse
        {
            Username = user.UserName,
            Email = user.Email,
            Token = token
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
        
        // push token to RebbitMQ
        
        return Result<string>.Success("Password reset successfully");
    }

    public async Task<Result<string>> ResetPasswordAsync(ResetPsswordDto request, string token)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null)
        {
            return Result<string>.Failure($"{request.Email} - this email address is not registered.");
        }
        
        var resetPassResult = await _userManager.ResetPasswordAsync(user, token, request.NewPassword!);

        if(!resetPassResult.Succeeded)
        {
            var errors = resetPassResult.Errors.Select(e => e.Description);
            return Result<string>.Failure(errors.ToString()!);
        }
        
        return Result<string>.Success("Password reset successfully");
    }
}

