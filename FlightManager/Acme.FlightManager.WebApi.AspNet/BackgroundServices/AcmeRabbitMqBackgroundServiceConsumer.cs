using Acme.Base.Messaging.RabbitMq;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client.Events;
using System.Threading;
using System.Threading.Tasks;

namespace Acme.FlightManager.WebApi.AspNet.BackgroundServices;

public class AcmeRabbitMqBackgroundServiceConsumer : BackgroundService
{
    private readonly AcmeRabbitMqConfiguration _configuration;

    public AcmeRabbitMqBackgroundServiceConsumer(AcmeRabbitMqConfiguration configuration) =>
        _configuration = configuration;

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _configuration.Channel.BasicQos(0, 1, false);
        var consumer = new EventingBasicConsumer(_configuration.Channel);

        consumer.Received += MessageReceived;

        return Task.CompletedTask;
    }

    private void MessageReceived(object sender, BasicDeliverEventArgs e)
    {
        //var bla = e.Body.GetMessage<AddedAirplaneToTheFleetEvent>();
        // deserijaliziraj poruku, spremi u bazu, ack message, nazovi servis po tome, postavi u DestinationDirector DLL
        throw new System.NotImplementedException();
    }
}