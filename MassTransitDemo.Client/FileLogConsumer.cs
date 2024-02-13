using MassTransit;
using MassTransitDemo.Abstract;

namespace MassTransitDemo.Client;

public class FileLogConsumer : IConsumer<Log>
{
    private readonly ILogger<FileLogConsumer> _logger;

    public FileLogConsumer(ILogger<FileLogConsumer> logger)
    {
        _logger = logger;
    }
    public Task Consume(ConsumeContext<Log> context)
    {
        _logger.LogInformation($"FILE LOG recieved {context.Message.Content}");
        return Task.CompletedTask;
    }
}

public class FileLogConsumerDefinition : ConsumerDefinition<FileLogConsumer>
{
    public FileLogConsumerDefinition()
    {
        EndpointName = "log-file";
    }

    protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<FileLogConsumer> consumerConfigurator,
        IRegistrationContext context)
    {
        endpointConfigurator.ConfigureConsumeTopology = false;

        if (endpointConfigurator is IRabbitMqReceiveEndpointConfigurator rmq)
        {
            rmq.Bind("log-shared");
        }
    }
}