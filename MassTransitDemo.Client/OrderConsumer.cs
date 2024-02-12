using MassTransit;
using MassTransitDemo.Abstract;

namespace MassTransitDemo.Client;

public class OrderConsumer : IConsumer<SubmitOrder>
{
    public Task Consume(ConsumeContext<SubmitOrder> context)
    {
        var logger = context.GetServiceOrCreateInstance<ILogger<OrderConsumer>>();
        logger.LogInformation($"Order recieved {context.Message.Order}");
        return Task.CompletedTask;
    }
}