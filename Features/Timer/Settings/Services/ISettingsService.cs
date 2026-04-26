using Microsoft.AspNetCore.Mvc;
using timer.Features.Timer.Settings.Dto;

namespace timer.Features.Timer.Settings.Services;

public interface ISettingsService
{
    Task<SettingsResponse> GetTimerSettings(Guid userId);
    Task<SettingsResponse> SetTimerSettings(Guid userId, SettingsRequest request);
}
