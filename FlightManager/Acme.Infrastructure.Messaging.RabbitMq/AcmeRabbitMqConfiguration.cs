using Acme.SharedKernel.Domain.Entity;
using Acme.SharedKernel.Domain.Messaging;
using RabbitMQ.Client;
using System;
using System.Text;

namespace Acme.Base.Messaging.RabbitMq;

public class AcmeRabbitMqConfiguration
{
    protected readonly IConnection _connection;

    public IModel Channel { get; private set; }
    public RabbitMqConfiguration Configuration { get; private set; }

    // in case of multiple exchanges or queues, a dictionary or a tuple could combine exchanges, queues and routing keys
    public AcmeRabbitMqConfiguration(ConnectionFactory connectionFactory, RabbitMqConfiguration configuration)
    {
        Configuration = configuration;
        _connection = connectionFactory.CreateConnection();

        Channel = CreateChannel();
    }

    private IModel CreateChannel()
    {
        var channel = _connection.CreateModel();
        channel.ExchangeDeclare(Configuration.ExchangeName, Configuration.Type, true, false);
        channel.QueueDeclare(Configuration.QueueName, true, true, false);
        channel.QueueBind(Configuration.QueueName, Configuration.ExchangeName, Configuration.RoutingKey);

        return channel;
    }

    public static ReadOnlyMemory<byte> GetMessage<TMessage>(TMessage message) where TMessage : IIntegrationEvent =>
        Encoding.UTF8.GetBytes(message.ConvertToString());

    public TMessage GetMessage<TMessage>(ReadOnlyMemory<byte> message) where TMessage : IIntegrationEvent =>
        Encoding.UTF8.GetString(message.ToArray()).ConvertToMessage<TMessage>();

    ~AcmeRabbitMqConfiguration() =>
        _connection.Close();
}