namespace TaskRunner;

public interface ISettings
{
    /// <summary>
    /// Safety interval of each task. Cannot rerun more often. 
    /// </summary>
    TimeSpan Interval { get; }
    
    /// <summary>
    /// Maximal count of simultaneously running tasks.
    /// </summary>
    int ConcurrentCount { get; }
}