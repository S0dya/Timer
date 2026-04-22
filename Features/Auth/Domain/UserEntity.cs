namespace timer.Features.Auth.Domain;

public class UserEntity
{
    public Guid Id { get; set; }
    public string UserEmail { get; set; }
    public string Username { get; set; }
    public string PasswordHash { get; set; }
    public UserRole UserRole { get; set; }
    public DateTime CreatedAt { get; set; }
}

public enum UserRole
{
    User,
    Admin,
}