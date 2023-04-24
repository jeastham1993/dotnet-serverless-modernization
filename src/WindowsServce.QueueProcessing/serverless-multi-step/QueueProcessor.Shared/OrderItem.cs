namespace QueueProcessor.Shared;

public class OrderItem
{
    public string ProductCode { get; set; }

    public int Quantity { get; set; }
    
    public bool InStock { get; set; }
}