using MassTransit;

namespace MassTransitDemo.Abstract;

[MessageUrn("log")]
public class Log
{
    public string Content { get; set; }
}