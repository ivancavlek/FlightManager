using Acme.Base.Domain;
using Acme.Base.Domain.Entity;
using Acme.Base.Domain.Messaging;
using RabbitMQ.Client;
using System;
using System.Text;

namespace Acme.Base.Messaging.RabbitMq;

public abstract class RabbitMqBase
{
    protected readonly RabbitMqConfiguration _configuration;
    protected readonly IConnection _connection;

    // in case of multiple exchanges or queues, a dictionary or a tuple could combine exchanges, queues and routing keys
    protected RabbitMqBase(ConnectionFactory connectionFactory, RabbitMqConfiguration configuration)
    {
        _configuration = configuration;
        _connection = connectionFactory.CreateConnection();
    }

    protected IModel CreateChannel()
    {
        var channel = _connection.CreateModel();
        channel.ExchangeDeclare(_configuration.ExchangeName, _configuration.Type, true, false);
        channel.QueueDeclare(_configuration.QueueName, true, true, false);
        channel.QueueBind(_configuration.QueueName, _configuration.ExchangeName, _configuration.RoutingKey);

        return channel;
    }

    protected static ReadOnlyMemory<byte> GetMessage<TMessage>(TMessage message) where TMessage : IIntegrationEvent =>
        Encoding.UTF8.GetBytes(message.ConvertToString());

    ~RabbitMqBase() =>
        _connection.Close();
}