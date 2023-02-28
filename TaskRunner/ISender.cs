namespace TaskRunner;

public interface ISender<in T>
{
    Task SendAsync(T item);
}