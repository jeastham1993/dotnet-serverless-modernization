using System.Collections.Generic;

namespace QueueProcessor.Shared;

public class Order
{
    public string CustomerName { get; set; }

    public List<OrderItem> Items { get; set; }
    
    public decimal Price { get; set; }
}