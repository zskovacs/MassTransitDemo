using System.Reflection.PortableExecutable;
using MassTransit;
using MassTransitDemo.Abstract;
using MassTransitDemo.Server;
using RabbitMQ.Client;
using Serilog;

var builder = WebApplication.CreateBuilder();
builder.Services.AddHostedService<MessagePublisher>();

builder.Logging.ClearProviders();
var logger = new LoggerConfiguration()
    .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
    .CreateLogger();
builder.Logging.AddSerilog(logger);

builder.Services.AddMassTransit(x =>
{
    x.SetKebabCaseEndpointNameFormatter();
    
    x.AddConsumers(typeof(Program).Assembly);
    
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("localhost", "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });
        
        cfg.Message<SubmitOrder>(x => x.SetEntityName("submitorder"));
        cfg.Publish<SubmitOrder>(x =>
        {
            x.ExchangeType = ExchangeType.Direct;
        });
        cfg.Send<SubmitOrder>(x =>
        {
            x.UseCorrelationId(context => context.TransactionId);
            x.UseRoutingKeyFormatter(context => context.Message.CustomerType);
        });
        
        
        cfg.ConfigureEndpoints(context);
    });
});


var app = builder.Build();

app.Run();