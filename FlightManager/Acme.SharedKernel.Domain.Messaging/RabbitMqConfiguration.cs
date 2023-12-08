namespace Acme.SharedKernel.Domain.Messaging;

public record RabbitMqConfiguration(string ExchangeName, string QueueName, string RoutingKey, string Type);