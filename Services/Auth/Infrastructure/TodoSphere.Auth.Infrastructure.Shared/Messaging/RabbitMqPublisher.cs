using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using TodoSphere.Auth.Application.Interfaces.Services;
using TodoSphere.Auth.Application.Utils;

namespace TodoSphere.Auth.Infrastructure.Shared.Messaging;

public class RabbitMqPublisher(IConnection connection) : IMessagePublisher
{
    public Task<Result> PublishAsync<T>(T message, string queueName) where T : class
    {
        using var channel = connection.CreateModel();

        channel.QueueDeclare(
            queue: queueName,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null);

        var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));

        var properties = channel.CreateBasicProperties();
        properties.DeliveryMode = 2;
        properties.ContentType = "application/json";

        channel.ConfirmSelect();

        channel.BasicPublish(
            exchange: "",
            routingKey: queueName,
            mandatory: true,
            basicProperties: properties,
            body: body);

        var confirmed = channel.WaitForConfirms(TimeSpan.FromSeconds(5));

        if (!confirmed)
            return Task.FromResult(Result.Failure(Error.InternalServerError("500",
                $"RabbitMQ did not confirm the message published to '{queueName}'.")));

        return Task.FromResult(Result.Success());
    }
}