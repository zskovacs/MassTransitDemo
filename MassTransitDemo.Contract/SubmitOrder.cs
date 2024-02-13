using MassTransit;

namespace MassTransitDemo.Abstract;

[MessageUrn("submit-order")]
public record SubmitOrder
{
    public string CustomerType { get; init; }
    public Guid TransactionId { get; init; }
    
    
    public string Order { get; init; }
}