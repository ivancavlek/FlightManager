using Acme.Base.Domain.Messaging;
using RabbitMQ.Client;

namespace Acme.Base.Messaging.RabbitMq;

public sealed class AcmeRabbitMqPublisher : RabbitMqBase, IMessagePublisher
{
    public AcmeRabbitMqPublisher(ConnectionFactory connectionFactory, RabbitMqConfiguration configuration)
        : base(connectionFactory, configuration) { }

    void IMessagePublisher.PublishMessage<TMessage>(TMessage message)
    {
        var channel = CreateChannel();

        channel.BasicPublish(
            _configuration.ExchangeName, _configuration.RoutingKey, body: GetMessage(message));

        channel.Close();
    }
}