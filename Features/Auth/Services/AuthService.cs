using System.Security.Authentication;
using Microsoft.EntityFrameworkCore;
using timer.Database;
using timer.Features.Auth.CurrentUser;
using timer.Features.Auth.Domain;
using timer.Features.Auth.Dto;
using timer.Features.Auth.Jwt;

namespace timer.Features.Auth.Services;

public class AuthService : IAuthService
{
    private readonly ILogger<AuthService> _logger;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly AppDbContext _db;

    public AuthService(ILogger<AuthService> logger, 
        IJwtTokenGenerator jwtTokenGenerator,
        AppDbContext db)
    {
        _logger = logger;
        _jwtTokenGenerator = jwtTokenGenerator;
        _db = db;
    }
    
    public async Task<AuthResponse> Login(LoginRequest request)
    {
        _logger.LogInformation("Login attempt for email {Email}", request.Email);

        var existingUser = await _db.Users.FirstOrDefaultAsync(user => user.UserEmail == request.Email);

        if (existingUser == null)
        {
            _logger.LogWarning("Login failed for email {Email}: user not found", request.Email);
            throw new AuthenticationException("Invalid Credentials");
        }
        if (!BCrypt.Net.BCrypt.Verify(request.Password, existingUser.PasswordHash))
        {
            _logger.LogWarning("Login failed for email {Email}: invalid password", request.Email);
            throw new AuthenticationException("Invalid Credentials");
        }

        _logger.LogInformation("Login succeeded for user {UserId}", existingUser.Id);

        return new ()
        {
            Token = _jwtTokenGenerator.GenerateToken(new JwtUser()
            {
                Id = existingUser.Id,
                Username = existingUser.Username,
                UserRole = existingUser.UserRole,
            }),
        };
    }

    public async Task<AuthResponse> Register(RegisterRequest request)
    {
        _logger.LogInformation("Registration attempt for email {Email}", request.Email);

        var existingUser = await _db.Users.FirstOrDefaultAsync(user => user.UserEmail == request.Email);

        if (existingUser != null)
        {
            _logger.LogWarning("Registration failed for email {Email}: user already exists", request.Email);
            throw new AuthenticationException("User Already Exists");
        }

        var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

        var newUser = new UserEntity()
        {
            Id = Guid.NewGuid(),
            UserEmail = request.Email,
            Username = "User",
            PasswordHash = passwordHash,
            UserRole = UserRole.User,
            CreatedAt = DateTime.UtcNow,
        };
        
        _db.Users.Add(newUser);
        await _db.SaveChangesAsync();

        _logger.LogInformation("Registration succeeded for user {UserId}", newUser.Id);
        
        return new ()
        {
            Token = _jwtTokenGenerator.GenerateToken(new JwtUser()
            {
                Id = newUser.Id,
                Username = newUser.Username,
                UserRole = newUser.UserRole,
            }),
        };
    }
}