using TodoSphere.Auth.Application.Utils;

namespace TodoSphere.Auth.Application.Interfaces.Services;

public interface IMessagePublisher
{
    /// <summary>
    /// Publishes a message to the specified routing key for further processing.
    /// </summary>
    /// <typeparam name="T">The type of the class representing the data associated with the message.</typeparam>
    /// <param name="message">The content of the message to be published.</param>
    /// <param name="routingKey">The routing key used to route the message appropriately.</param>
    /// <returns>A task that represents the asynchronous publish operation.</returns>
    Task<Result> PublishAsync<T>(T message, string routingKey) where T : class;
}