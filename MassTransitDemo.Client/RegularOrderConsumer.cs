using MassTransit;
using MassTransitDemo.Abstract;
using RabbitMQ.Client;

namespace MassTransitDemo.Client;

public class RegularOrderConsumer : IConsumer<SubmitOrder>
{
    private readonly ILogger<RegularOrderConsumer> _logger;

    public RegularOrderConsumer(ILogger<RegularOrderConsumer> logger)
    {
        _logger = logger;
    }
    public Task Consume(ConsumeContext<SubmitOrder> context)
    {
        _logger.LogInformation($"Order recieved {context.Message.Order}");
        return Task.CompletedTask;
    }
}

public class RegularOrderConsumerDefinition : ConsumerDefinition<RegularOrderConsumer>
{
    public RegularOrderConsumerDefinition()
    {
        EndpointName = "order-priority";
    }

    protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<RegularOrderConsumer> consumerConfigurator,
        IRegistrationContext context)
    {
        endpointConfigurator.ConfigureConsumeTopology = false;

        if (endpointConfigurator is IRabbitMqReceiveEndpointConfigurator rmq)
        {
            rmq.Bind("submit-order", b =>
            {
                b.RoutingKey = "REGULAR";
                b.ExchangeType = ExchangeType.Direct;
            });
        }
    }
}