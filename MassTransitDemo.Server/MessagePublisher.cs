using MassTransit;
using MassTransitDemo.Abstract;

namespace MassTransitDemo.Server;

public class MessagePublisher(IBus bus) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Yield();

            var pressed = Console.ReadKey(true);

            
            if (pressed.Key == ConsoleKey.Q)
            {
                await bus.Publish(new SubmitOrder() { TransactionId = InVar.Id, CustomerType = "REGULAR", Order = "Fagyi"}, stoppingToken);
            }
            
            if (pressed.Key == ConsoleKey.E)
            {
                await bus.Publish(new SubmitOrder() { TransactionId = InVar.Id, CustomerType = "PRIORITY", Order = "Kávé"}, stoppingToken);
            }


            await Task.Delay(1000, stoppingToken);
        }
    }
}