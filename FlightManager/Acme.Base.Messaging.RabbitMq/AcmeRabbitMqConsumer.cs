using Acme.Base.Domain.Messaging;
using RabbitMQ.Client;

namespace Acme.Base.Messaging.RabbitMq;

public class AcmeRabbitMqConsumer : RabbitMqBase, IMessageConsumer
{
    public AcmeRabbitMqConsumer(ConnectionFactory connectionFactory, RabbitMqConfiguration configuration)
        : base(connectionFactory, configuration) { }
}