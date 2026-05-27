using timer.Features.Auth.CurrentUser;
using timer.Features.Auth.Jwt;
using timer.Features.Auth.Services;
using timer.Features.Auth.Validation;
using timer.Features.Timer.ActiveRun.Services;
using timer.Features.Timer.Run.Services;
using timer.Features.Timer.Settings.Services;
using timer.Options;

namespace Timer.Infrastructure.DependencyInjection;

public static class TimerInjection
{
    public static IServiceCollection AddTimerServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtOptions>(configuration.GetSection("Jwt"));
        services.Configure<TimerSettingsOptions>(configuration.GetSection("TimerSettings"));

        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
        services.AddScoped<ICurrentUser, CurrentUser>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IRunService, DbRunService>();
        services.AddScoped<ISettingsService, DbSettingsService>();
        services.AddScoped<IActiveRunService, DbActiveRunService>();
        services.AddScoped<IPasswordValidation, PasswordValidation>();

        return services;
    }
}
