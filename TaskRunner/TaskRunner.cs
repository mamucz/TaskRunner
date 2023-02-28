using System.Collections.Concurrent;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace TaskRunner;

public sealed class TaskRunner<T>: ITaskRunner<T> where T : IEquatable<T>
{
    private readonly ConcurrentDictionary<T, TaskRunnerItem<T>> _items = new();
    private readonly ISender<T> _sender;
    private readonly ISettings _settings; 

    private int _registeredCount;
    private int _runningCount;
    public int RunningCount => _runningCount;

    public TaskRunner( 
        ISender<T> sender,
        ISettings settings 
    )
    {
        ArgumentNullException.ThrowIfNull(sender);
        ArgumentNullException.ThrowIfNull(settings);

        _sender = sender;
        _settings = settings; 
    }

    public void Add(T key)
    {
        if (_items.TryGetValue(key, out var item))
        {
            if (item.TryRegister())
                Interlocked.Increment(ref _registeredCount);
        }
        else
        {
            _items.TryAdd(key, new(key));
            if (_items.TryGetValue(key, out item) &&
                item.TryRegister())
                Interlocked.Increment(ref _registeredCount);
        }

        Invoke();
    }

    public void Remove(T key)
    {
        if (_items.TryGetValue(key, out var item))
            item.TryRemoveRegisteredDuringRun();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void Invoke()
    {
        while (true)
        {
            if (!TryGetOldest(out var item)) return;
            if (Interlocked.Increment(ref _runningCount) > _settings.ConcurrentCount)
                Interlocked.Decrement(ref _runningCount);
            else
            {
                if (item.TryRun())
                    Task.Run(() => InvokeAsync(item));
                else
                {
                    Interlocked.Decrement(ref _runningCount);
                    continue;
                }
            }

            break;
        }
    }


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private bool TryGetOldest([NotNullWhen(true)] out TaskRunnerItem<T>? item)
    {
        var min = long.MaxValue;
        item = null;
        foreach (var kvp in _items)
        {
            if (kvp.Value.IsRunningOrNotRegistered()) continue;
            if (!kvp.Value.TryGetLastStart(out var last))
            {
                item = kvp.Value;
                return true;
            }

            if (min <= last) continue;
            min = last;
            item = kvp.Value;
        }

        return item is not null;
    }

    private async Task InvokeAsync(TaskRunnerItem<T> item)
    {
        var timeCpu = Stopwatch.GetTimestamp();
        item.SetTimeCpuLastStart(timeCpu);
        await _sender.SendAsync(item.Key);
        var delay = TimeSpan.FromSeconds((double)(Stopwatch.GetTimestamp() - timeCpu) / Stopwatch.Frequency);
        if (delay < _settings.Interval)
            await Task.Delay(_settings.Interval - delay);

        item.RemoveRunning();
        Interlocked.Decrement(ref _runningCount);
        if (item.TryRemoveRegisteredDuringRun())
            Invoke();
        else if (item.TryRemoveRegistered())
        {
            if (Interlocked.Decrement(ref _registeredCount) > 0)
                Invoke();
        }
        else if (_registeredCount != 0)
            Invoke();

        if (_registeredCount == 0)
            Clean();
    }

    private void Clean()
    {
        foreach (var item in _items)
        {
            if (item.Value.IsRegistered() ||
                !item.Value.TryGetLastStart(out var lastStart) ||
                TimeSpan.FromSeconds((double)(Stopwatch.GetTimestamp() - lastStart) / Stopwatch.Frequency) <= 2 * _settings.Interval)
                continue;
            if (_items.TryRemove(item.Key, out var removed) &&
                removed.IsRegistered())
                Add(removed.Key);
        }
    }
}