using Acme.SharedKernel.Domain.Messaging;
using RabbitMQ.Client;

namespace Acme.Base.Messaging.RabbitMq;

public sealed class AcmeRabbitMqPublisher : IMessagePublisher
{
    private readonly AcmeRabbitMqConfiguration _configuration;

    public AcmeRabbitMqPublisher(AcmeRabbitMqConfiguration acmeRabbitMqConfiguration) =>
        _configuration = acmeRabbitMqConfiguration;


    void IMessagePublisher.PublishMessage<TMessage>(TMessage message)
    {
        _configuration.Channel.BasicPublish(
            _configuration.Configuration.ExchangeName,
            _configuration.Configuration.RoutingKey,
            body: AcmeRabbitMqConfiguration.GetMessage(message));

        _configuration.Channel.Close();
    }
}