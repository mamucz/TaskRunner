namespace TaskRunner.Test;

public sealed class MockSettings : ISettings
{
    public TimeSpan Interval { get; } = TimeSpan.FromMilliseconds(197);
    public int ConcurrentCount { get; } = 3;
}