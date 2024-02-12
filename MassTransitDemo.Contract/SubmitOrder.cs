namespace MassTransitDemo.Abstract;

public record SubmitOrder
{
    public string CustomerType { get; init; }
    public Guid TransactionId { get; init; }
    
    
    public string Order { get; init; }
}