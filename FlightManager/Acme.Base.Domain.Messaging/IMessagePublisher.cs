using Acme.Base.Domain.Entity;

namespace Acme.Base.Domain.Messaging;

public interface IMessagePublisher
{
    void PublishMessage<TMessage>(TMessage message) where TMessage : IIntegrationEvent;
}