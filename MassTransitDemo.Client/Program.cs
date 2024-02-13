using MassTransit;
using MassTransitDemo.Abstract;
using MassTransitDemo.Client;
using RabbitMQ.Client;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder();

builder.Logging.ClearProviders();
var logger = new LoggerConfiguration()
    .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
    .CreateLogger();
builder.Logging.AddSerilog(logger);


builder.Services.AddMassTransit(x =>
{
    x.SetKebabCaseEndpointNameFormatter();
    
    //DIRECT
    x.AddConsumer<PriorityOrderConsumer, PriorityOrderConsumerDefinition>();
    x.AddConsumer<RegularOrderConsumer, RegularOrderConsumerDefinition>();
    
    // FANOUT
    x.AddConsumer<FileLogConsumer, FileLogConsumerDefinition>();
    x.AddConsumer<ConsoleLogConsumer, ConsoleLogConsumerDefinition>();
    
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("localhost", "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });

        cfg.ConfigureEndpoints(context);
    });
});


var app = builder.Build();

app.Run();