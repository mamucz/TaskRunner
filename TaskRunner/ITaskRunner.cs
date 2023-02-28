namespace TaskRunner;

/// <summary>
/// Run up to <c>ISettings.ConcurrentCount</c> tasks that awaits <c>ISender.SendAsync</c> method.
/// Equal items can be running only in one task.
/// Allows rerun of the equal items only after <c>ISettings.Interval</c> from task start. 
/// When multiple equal items are added during task processing and
/// processing ends or <c>ISettings.Interval</c> passes, task will run only once again. 
/// </summary> 
public interface ITaskRunner<in T> where T : IEquatable<T>
{
    /// <summary>
    /// Currently running tasks count.
    /// </summary>
    int RunningCount { get; }
    
    /// <summary>
    /// Adds an item for processing.
    /// </summary>
    void Add(T item);
    
    /// <summary>
    /// Removes an item from future processing.
    /// </summary> 
    void Remove(T key);
}