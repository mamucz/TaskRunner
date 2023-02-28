namespace TaskRunner;

public sealed class TaskRunnerItem<T>
{
    public T Key { get; }
    private int _registered;
    private int _registeredDuringRun;
    private int _running;
    private long _timeCpuLastStart;

    public TaskRunnerItem(T key) => Key = key;
     
    public bool IsRegistered() => _registered == 1;
    public bool IsRunningOrNotRegistered() => _running == 1 || _registered == 0;
    public void SetTimeCpuLastStart(long timeCpuLastStart) => _timeCpuLastStart = timeCpuLastStart;
    public bool TryRun() => _registered == 1 && Interlocked.CompareExchange(ref _running, 1, 0) == 0;
    public void RemoveRunning() => _running = 0;
    public bool TryRemoveRegisteredDuringRun() => Interlocked.CompareExchange(ref _registeredDuringRun, 0, 1) == 1;
    public bool TryRemoveRegistered() => Interlocked.CompareExchange(ref _registered, 0, 1) == 1;

    public bool TryGetLastStart(out long timeCpuLastStart)
    {
        timeCpuLastStart = _timeCpuLastStart;
        return timeCpuLastStart != 0;
    }

    public bool TryRegister()
    {
        if (Interlocked.CompareExchange(ref _registered, 1, 0) == 0) return true;
        Interlocked.CompareExchange(ref _registeredDuringRun, 1, 0);
        return false;
    } 
}