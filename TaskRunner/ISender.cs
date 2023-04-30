namespace TaskRunner;
/// <summary>
/// Interface for sending request
/// </summary>
/// <typeparam name="T">Obejct to send</typeparam>
public interface ISender<in T>
{
    /// <summary>
    /// Send the object async
    /// </summary>
    /// <param name="item">object to send</param>
    /// <returns>return task</returns>
    Task SendAsync(T item);
}