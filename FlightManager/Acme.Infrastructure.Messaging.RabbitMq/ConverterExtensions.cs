using Acme.SharedKernel.Domain.Entity;
using System.Text.Json;

namespace Acme.Base.Messaging.RabbitMq;

public static class ConverterExtensions
{
    public static string ConvertToString(this IIntegrationEvent integrationEvent) =>
        JsonSerializer.Serialize(integrationEvent);

    public static TMessage ConvertToMessage<TMessage>(this string message)
        where TMessage : IIntegrationEvent =>
        JsonSerializer.Deserialize<TMessage>(message);
}