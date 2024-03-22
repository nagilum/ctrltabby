namespace CtrlTabby;

internal class Options
{
    /// <summary>
    /// Whether to start the process automatically.
    /// </summary>
    public bool AutoStart { get; set; }
    
    /// <summary>
    /// Interval between sends, in milliseconds.
    /// </summary>
    public decimal Interval { get; set; } = 3000;

    /// <summary>
    /// Whether to stop if interrupted by system events.
    /// </summary>
    public bool StopIfInterrupted { get; set; } = true;
}