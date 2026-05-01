namespace timer.Features.Auth.Validation;

public interface IPasswordValidation
{
    void ValidatePassword(string password);
}