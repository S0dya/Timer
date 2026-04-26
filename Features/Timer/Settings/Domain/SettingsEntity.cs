namespace timer.Features.Timer.Settings.Domain;

public class SettingsEntity
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public int SessionDuration { get; set; }
    public int SessionsAmount { get; set; }
}
