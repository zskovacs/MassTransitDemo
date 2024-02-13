using System.Reflection.PortableExecutable;
using MassTransit;
using MassTransitDemo.Abstract;
using MassTransitDemo.Server;
using RabbitMQ.Client;
using Serilog;
using Log = MassTransitDemo.Abstract.Log;

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
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("localhost", "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });
        
        cfg.Message<SubmitOrder>(x => x.SetEntityName("submit-order"));
        cfg.Publish<SubmitOrder>(x =>
        {
            x.ExchangeType = ExchangeType.Direct;
        });
        cfg.Send<SubmitOrder>(x =>
        {
            x.UseCorrelationId(c => c.TransactionId);
            x.UseRoutingKeyFormatter(c => c.Message.CustomerType);
        });
        
        cfg.Message<Log>(x => x.SetEntityName("log-shared"));
        
        cfg.ConfigureEndpoints(context);
    });
});


var app = builder.Build();

app.Run();