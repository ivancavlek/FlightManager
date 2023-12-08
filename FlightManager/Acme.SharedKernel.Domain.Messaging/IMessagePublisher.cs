using Acme.SharedKernel.Domain.Entity;

namespace Acme.SharedKernel.Domain.Messaging;

public interface IMessagePublisher
{
    void PublishMessage<TMessage>(TMessage message) where TMessage : IIntegrationEvent;
}