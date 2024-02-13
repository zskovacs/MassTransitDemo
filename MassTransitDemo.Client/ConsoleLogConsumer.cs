using MassTransit;
using MassTransitDemo.Abstract;

namespace MassTransitDemo.Client;

public class ConsoleLogConsumer : IConsumer<Log>
{
    private readonly ILogger<ConsoleLogConsumer> _logger;

    public ConsoleLogConsumer(ILogger<ConsoleLogConsumer> logger)
    {
        _logger = logger;
    }
    public Task Consume(ConsumeContext<Log> context)
    {
        _logger.LogInformation($"CONSOLE LOG recieved {context.Message.Content}");
        return Task.CompletedTask;
    }
}

public class ConsoleLogConsumerDefinition : ConsumerDefinition<ConsoleLogConsumer>
{
    public ConsoleLogConsumerDefinition()
    {
        EndpointName = "log-console";
    }

    protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<ConsoleLogConsumer> consumerConfigurator,
        IRegistrationContext context)
    {
        endpointConfigurator.ConfigureConsumeTopology = false;

        if (endpointConfigurator is IRabbitMqReceiveEndpointConfigurator rmq)
        {
            rmq.Bind("log-shared");
        }
    }
}