namespace TaskRunner.Test;

public sealed class MockSender<T> : ISender<T>
{
    public EventHandler<T>? OnSendAsyncCalled { get; set; }
    public Task? SendAsyncReturn { get; set; }
    
    public Task SendAsync(T item)
    {
        OnSendAsyncCalled?.Invoke(this, item);
        return SendAsyncReturn!;
    }
}