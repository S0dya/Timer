namespace timer.Options;

public class TimerSettingsOptions
{
    public int DefaultDuration { get; set; } = default!;
    public int DefaultAmount { get; set; } = default!;
    
    public int MinDuration { get; set; } = default!;
    public int MinAmount { get; set; } = default!;
    public int MaxDuration { get; set; } = default!;
    public int MaxAmount { get; set; } = default!;
    public int SessionStopTimerDifferenceOffset { get; set; } = default!;

    public int MaxRunsHistoryLimit { get; set; } = default!;
}