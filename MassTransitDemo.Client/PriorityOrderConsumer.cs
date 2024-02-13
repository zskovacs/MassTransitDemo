using MassTransit;
using MassTransitDemo.Abstract;
using RabbitMQ.Client;

namespace MassTransitDemo.Client;

public class PriorityOrderConsumer : IConsumer<SubmitOrder>
{
    private readonly ILogger<RegularOrderConsumer> _logger;

    public PriorityOrderConsumer(ILogger<RegularOrderConsumer> logger)
    {
        _logger = logger;
    }
    public Task Consume(ConsumeContext<SubmitOrder> context)
    {
        _logger.LogInformation($"Order recieved {context.Message.Order}");
        return Task.CompletedTask;
    }
}


public class PriorityOrderConsumerDefinition : ConsumerDefinition<PriorityOrderConsumer>
{
    public PriorityOrderConsumerDefinition()
    {
        EndpointName = "order-regular";
    }

    protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<PriorityOrderConsumer> consumerConfigurator,
        IRegistrationContext context)
    {
        endpointConfigurator.ConfigureConsumeTopology = false;

        if (endpointConfigurator is IRabbitMqReceiveEndpointConfigurator rmq)
        {
            rmq.Bind("submit-order", b =>
            {
                b.RoutingKey = "PRIORITY";
                b.ExchangeType = ExchangeType.Direct;
            });
            
        }
    }
}